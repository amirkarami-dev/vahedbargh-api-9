using Coreapi.Application.Common.Interfaces;
using Coreapi.Common.Enums;
using Coreapi.Common.Utility;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using Coreapi.Domain.AggregatesModel.FinanceAgg;
using MediatR;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Coreapi.Application.Features.ElectProjects.Commands.CreateBigProjectChildren;

/// <summary>
/// Handles creation of child (sub-project) records for a big project.
/// Supports both inspection children and ERT children.
/// Pre-cleanup: deletes any existing children (with their invoices and transactions)
/// before creating new ones — safe to call idempotently.
/// After child creation:
///   1. Updates electProject.AmountPerArea to the final payWithSms total.
///   2. If the parent project already has a paid (TransactionStatusEnum.In) transaction
///      whose amount equals payWithSms, the parent-level In transaction is deleted and
///      redistributed across the newly created children (mirrors PaymentMelliPublicReturn
///      big-project payment flow).
/// NOTE: Does NOT call SaveChangesAsync; callers are responsible for saving.
/// </summary>
public class CreateBigProjectChildrenCommandHandler(
    IElectProjectRepository electProjectRepository,
    IElectProjectErtFormRepository projectErtFormRepository,
    IInvoiceRepository invoiceRepository,
    ITransactionRepository transactionRepository,
    ICurrentUser currentUser
) : IRequestHandler<CreateBigProjectChildrenCommand, long>
{
    public async Task<long> Handle(CreateBigProjectChildrenCommand request, CancellationToken cancellationToken)
    {
        long payWithSms = 0;
        var electProject = request.ElectProject;
        var client = request.Client;
        var buildingTariff = request.BuildingTariff;
        var ertTariff = request.ErtTariff;
        var fileNumberChild = request.StartFileNumber;
        var childInspectionCount = request.ChildInspectionCount;
        var childErtCount = request.ChildErtCount;

        // Track newly created children in-memory so we can redistribute
        // an existing In transaction without waiting for SaveChanges.
        var createdInspectionChildren =
            new List<(ElectProject Project, Invoice InspectionInvoice, Invoice ServiceInvoice)>();
        var createdErtChildren =
            new List<(ElectProject Project, Invoice ErtInvoice, Invoice ServiceInvoice)>();

        // ── Pre-cleanup ──────────────────────────────────────────────────────
        // If this project already has children (edge case on re-call), delete
        // their Out transactions and invoices, then delete the child projects.
        if (electProject.ChildProjects is not null && electProject.ChildProjects.Any())
        {
            foreach (var child in electProject.ChildProjects.ToList())
            {
                foreach (var invoiceType in new[]
                         {
                             InvoicePayTypeEnum.CreateProjectStage,
                             InvoicePayTypeEnum.NezamStage,
                             InvoicePayTypeEnum.ProjectServices
                         })
                {
                    var invoice = await invoiceRepository.GetInvoiceByProjectId(child.Id, invoiceType);
                    if (invoice is null) continue;

                    // Delete Invoice first (releases the TransactionId FK),
                    // then delete the Transaction (no longer referenced).
                    await invoiceRepository.DeleteById(invoice.Id);
                    if (invoice.Transaction is not null)
                        await transactionRepository.DeleteById(invoice.Transaction.Id);
                }

                // ElectProjectErtForm cascades via DB config.
                await electProjectRepository.DeleteById(child.Id);
            }
        }

        // ── Create inspection children ────────────────────────────────────────
        if (childInspectionCount > 0)
        {
            var amountPayInspection = Helper.GetAmountInspection(
                buildingTariff.Tariff, request.Area, buildingTariff.Factor, buildingTariff.MinTariff);

            var amountPaySupervision = request.HasSupervision
                ? Helper.GetAmountSupervision(
                    buildingTariff.SupervisionTariff, request.AreaAsBuilt, buildingTariff.SupervisionFactor)
                : 0;

            var sumAmountService = Helper.GetAmountProjectServices(amountPayInspection + amountPaySupervision);

            long amountPayInspectionChild = amountPayInspection / childInspectionCount;
            long amountPayInspectionChildDiv = amountPayInspection % childInspectionCount;
            long amountPayServiceChild = sumAmountService / childInspectionCount;
            long amountPayServiceChildDiv = sumAmountService % childInspectionCount;
            long amountPaySupervisionChild = amountPaySupervision / childInspectionCount;
            long amountPaySupervisionChildDiv = amountPaySupervision % childInspectionCount;

            int areaChild = request.Area / childInspectionCount;
            int areaChildDiv = request.Area % childInspectionCount;
            int areaAsBuiltChild = request.AreaAsBuilt / childInspectionCount;
            int areaAsBuiltChildDiv = request.AreaAsBuilt % childInspectionCount;

            payWithSms += amountPayInspection + amountPaySupervision + sumAmountService;

            var remaining = childInspectionCount;
            while (remaining > 0)
            {
                // Last child absorbs the remainder from integer division
                if (remaining == 1)
                {
                    amountPayInspectionChild += amountPayInspectionChildDiv;
                    amountPayServiceChild += amountPayServiceChildDiv;
                    amountPaySupervisionChild += amountPaySupervisionChildDiv;
                    areaChild += areaChildDiv;
                    areaAsBuiltChild += areaAsBuiltChildDiv;
                }

                var childProject = new ElectProject(
                    fileNumberChild,
                    currentUser.UserId,
                    client,
                    areaChild,
                    request.NumberOfFloor,
                    $"{electProject.FileNumber}-{fileNumberChild - electProject.FileNumber}",
                    request.Address,
                    request.PostalCode,
                    request.LandlordName,
                    request.LandlordNaCode,
                    request.LandlordPhoneNumber,
                    electProject.CompanyName,
                    request.LicenseNumber,
                    electProject.Description,
                    request.IdSection,
                    electProject.IdCity,
                    electProject.IdProvince,
                    electProject.Lat,
                    electProject.Lng,
                    electProject.ProjectCreatedTypeEnum,
                    electProject.ProjectTypeRequestEnum,
                    buildingTariff,
                    null,
                    ProjectLevelEnum.NullStage,
                    DateTime.Now.Date,
                    Helper.MiladiToShamsi(DateTime.Now.Date),
                    false, false, true,
                    request.IsTestAndDelivery,
                    request.PanelNeed,
                    request.FoundationElectrodeArea,
                    false, 0,
                    request.HasRelatedPermit,
                    request.HasSupervision,
                    areaAsBuiltChild
                );
                childProject.UpdateParentProject(electProject);
                electProjectRepository.Add(childProject);

                // فاکتور بازرسی زیر پرونده
                var invoiceInspectionChild = new Invoice(
                    client, childProject,
                    amountPayInspectionChild + amountPaySupervisionChild,
                    InvoiceStatusEnum.Pending,
                    InvoicePayTypeEnum.CreateProjectStage);
                invoiceInspectionChild.UpdateAmountSupervision(amountPaySupervisionChild);
                invoiceRepository.Add(invoiceInspectionChild);

                var transactionChild = new Transaction(
                    invoiceInspectionChild.Amount, client, client.Id.ToString(),
                    GatewayTypeEnum.System, TransactionTypeEnum.Client, TransactionStatusEnum.Out, DateTime.Now,
                    Helper.MiladiToShamsi(DateTime.Now.Date), childProject.FileNumber.ToString(),
                    $"برداشت ایجاد پرونده بازرسی:{childProject.FileNumber}", childProject.Id.ToString());
                invoiceInspectionChild.Done(transactionChild);

                // فاکتور خدمات زیر پرونده
                var invoiceServices = new Invoice(
                    client, childProject, amountPayServiceChild,
                    InvoiceStatusEnum.Pending, InvoicePayTypeEnum.ProjectServices);
                invoiceRepository.Add(invoiceServices);

                var transactionServices = new Transaction(
                    amountPayServiceChild, client, client.Id.ToString(),
                    GatewayTypeEnum.System, TransactionTypeEnum.Client, TransactionStatusEnum.Out, DateTime.Now,
                    Helper.MiladiToShamsi(DateTime.Now.Date), childProject.FileNumber.ToString(),
                    $"برداشت هزینه خدمات:{childProject.FileNumber}", childProject.Id.ToString());
                invoiceServices.Done(transactionServices);

                // فرم ارت زیر پرونده بازرسی
                var ertFormChild = new ElectProjectErtForm(
                    childProject, "", 0, 0, "", ElectrodeUsageTypeEnum.None, "",
                    ElectrodeExecuteTypeEnum.None, "", ElectrodeTypeEnum.None, "",
                    ElectrodeMaterialTypeEnum.None, "", "", "", "", "", "", "", "", "", "", "", "", "");
                projectErtFormRepository.Add(ertFormChild);

                // Track for potential In-transaction redistribution
                createdInspectionChildren.Add((childProject, invoiceInspectionChild, invoiceServices));

                remaining--;
                fileNumberChild++;
            }
        }

        // ── Create ERT children ───────────────────────────────────────────────
        if (childErtCount > 0)
        {
            var amountPayErtSystem = Helper.GetAmountErtSystemType(
                ertTariff!.ErtSystemTypeEnum, ertTariff.Tariff, ertTariff.Factor,
                electProject.FoundationElectrodeArea);

            var amountPayService = Helper.GetAmountProjectServices(amountPayErtSystem);

            // Area division follows original Upsert behaviour:
            // uses ChildInspectionCount as divisor when both types are present.
            int divisor = childInspectionCount > 0 ? childInspectionCount : childErtCount;
            int areaChild = request.Area / divisor;
            int areaChildDiv = request.Area % divisor;

            payWithSms += (amountPayErtSystem * childErtCount) + (amountPayService * childErtCount);

            var remaining = childErtCount;
            while (remaining > 0)
            {
                if (remaining == 1) areaChild += areaChildDiv;

                var childErtProject = new ElectProject(
                    fileNumberChild,
                    currentUser.UserId,
                    client,
                    areaChild,
                    request.NumberOfFloor,
                    $"{electProject.FileNumber}-{fileNumberChild - electProject.FileNumber}",
                    request.Address,
                    request.PostalCode,
                    request.LandlordName,
                    request.LandlordNaCode,
                    request.LandlordPhoneNumber,
                    electProject.CompanyName,
                    request.LicenseNumber,
                    electProject.Description,
                    request.IdSection,
                    electProject.IdCity,
                    electProject.IdProvince,
                    electProject.Lat,
                    electProject.Lng,
                    electProject.ProjectCreatedTypeEnum,
                    electProject.ProjectTypeRequestEnum,
                    buildingTariff,
                    ertTariff,
                    ProjectLevelEnum.NullStage,
                    DateTime.Now.Date,
                    Helper.MiladiToShamsi(DateTime.Now.Date),
                    request.IsEarthSystem, false, false, false, false,
                    request.FoundationElectrodeArea,
                    request.IsNeedEb, 0,
                    request.HasRelatedPermit,
                    request.HasSupervision,
                    0
                );
                childErtProject.UpdateParentProject(electProject);
                electProjectRepository.Add(childErtProject);

                // فاکتور 9 درصد نظام زیر پرونده ارت
                var invoiceErtSystemChild = new Invoice(
                    client, childErtProject, amountPayErtSystem,
                    InvoiceStatusEnum.Pending, InvoicePayTypeEnum.NezamStage);
                invoiceRepository.Add(invoiceErtSystemChild);

                var transactionChild = new Transaction(
                    amountPayErtSystem, client, client.Id.ToString(),
                    GatewayTypeEnum.System, TransactionTypeEnum.Client, TransactionStatusEnum.Out, DateTime.Now,
                    Helper.MiladiToShamsi(DateTime.Now.Date), childErtProject.FileNumber.ToString(),
                    $"برداشت 9 درصد نظام:ارت:{childErtProject.FileNumber}", childErtProject.Id.ToString());
                invoiceErtSystemChild.Done(transactionChild);

                // فاکتور خدمات زیر پرونده ارت
                var invoiceServices = new Invoice(
                    client, childErtProject, amountPayService,
                    InvoiceStatusEnum.Pending, InvoicePayTypeEnum.ProjectServices);
                invoiceRepository.Add(invoiceServices);

                var transactionServices = new Transaction(
                    amountPayService, client, client.Id.ToString(),
                    GatewayTypeEnum.System, TransactionTypeEnum.Client, TransactionStatusEnum.Out, DateTime.Now,
                    Helper.MiladiToShamsi(DateTime.Now.Date), childErtProject.FileNumber.ToString(),
                    $"برداشت هزینه خدمات:{childErtProject.FileNumber}", childErtProject.Id.ToString());
                invoiceServices.Done(transactionServices);

                // فرم ارت زیر پرونده ارت
                var ertFormChild = new ElectProjectErtForm(
                    childErtProject, "", 0, 0, "", ElectrodeUsageTypeEnum.None, "",
                    ElectrodeExecuteTypeEnum.None, "", ElectrodeTypeEnum.None, "",
                    ElectrodeMaterialTypeEnum.None, "", "", "", "", "", "", "", "", "", "", "", "", "");
                projectErtFormRepository.Add(ertFormChild);

                // Track for potential In-transaction redistribution
                createdErtChildren.Add((childErtProject, invoiceErtSystemChild, invoiceServices));

                remaining--;
                fileNumberChild++;
            }
        }

        // ── Update AmountPerArea with the final complete total ───────────────
        // Sets IsBigProject = true and AmountPerArea = payWithSms in one call.
        electProject.UpdateBigProject(payWithSms);

        // ── Redistribute existing In payment to new children ─────────────────
        // If the parent project was already paid (a TransactionStatusEnum.In exists
        // whose amount matches the new payWithSms total), delete the parent-level
        // In transaction and re-create child-level In transactions — mirroring the
        // PaymentMelliPublicReturn big-project payment distribution flow.
        // Invoices are used from in-memory tracking (not queried) because
        // SaveChangesAsync has not been called yet.
        var existingInTransaction = await transactionRepository.GetByElectProjectId(
            electProject.Id.ToString(), TransactionStatusEnum.In);

        if (existingInTransaction is not null && existingInTransaction.Amount == payWithSms)
        {
            await transactionRepository.DeleteById(existingInTransaction.Id);

            // Clean up any leftover parent-level invoices/transactions before
            // creating the child-level In transactions below.
            var parentInvoices = (await invoiceRepository.GetInvoicesByProjectId(electProject.Id)).ToList();
            if (parentInvoices.Count > 0)
            {
                var invoiceIds = parentInvoices.Select(i => i.Id).ToList();
                var transactionIds = parentInvoices
                    .Where(i => i.Transaction is not null)
                    .Select(i => i.Transaction.Id)
                    .ToList();

                // Delete Invoices first (releases the TransactionId FK),
                // then delete the Transactions (no longer referenced).
                await invoiceRepository.DeleteByIds(invoiceIds);
                await transactionRepository.DeleteByIds(transactionIds);
            }

            foreach (var (childProject, invoiceInspection, invoiceService) in createdInspectionChildren)
            {
                var transactionInspectionChild = new Transaction(
                    invoiceInspection.Amount + invoiceService.Amount,
                    client, client.Id.ToString(),
                    existingInTransaction.GatewayType,
                    TransactionTypeEnum.Client,
                    TransactionStatusEnum.In,
                    DateTime.Now,
                    Helper.MiladiToShamsiFull(DateTime.Now),
                    existingInTransaction.BankTransactionId + "-" + childProject.FileNumber,
                    existingInTransaction.Des + " - کارشناسی" +
                    $" - پرونده اصلی:{electProject.FileNumber}" +
                    $" - مبلغ تراکنش اصلی:{electProject.AmountPerArea}",
                    childProject.Id.ToString()
                );
                transactionRepository.Add(transactionInspectionChild);
            }

            foreach (var (childErtProject, invoiceErt, invoiceService) in createdErtChildren)
            {
                var transactionErtChild = new Transaction(
                    invoiceErt.Amount + invoiceService.Amount,
                    client, client.Id.ToString(),
                    existingInTransaction.GatewayType,
                    TransactionTypeEnum.Client,
                    TransactionStatusEnum.In,
                    DateTime.Now,
                    Helper.MiladiToShamsiFull(DateTime.Now),
                    existingInTransaction.BankTransactionId + "-" + childErtProject.FileNumber,
                    existingInTransaction.Des + " - ارت" +
                    $" - پرونده اصلی:{electProject.FileNumber}" +
                    $" - مبلغ تراکنش اصلی:{electProject.AmountPerArea}",
                    childErtProject.Id.ToString()
                );
                transactionRepository.Add(transactionErtChild);
            }
        }

        return payWithSms;
    }
}