using System;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Common.Enums;
using Coreapi.Common.Utility;
using Coreapi.Domain.AggregatesModel.BuildingTariffAgg;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using Coreapi.Domain.AggregatesModel.EngineerAgg;
using Coreapi.Domain.AggregatesModel.FinanceAgg;
using Coreapi.Domain.AggregatesModel.QuarterTariffAgg;
using MediatR;

namespace Coreapi.Application.Features.ElectProjectProcesses.Commands.Accepted;

public class EngAcceptedCommandHandler(
    IElectProjectProcessRepository electProjectProcessRepository,
    IElectProjectRepository electProjectRepository,
    ICurrentUser currentUser,
    IClientRepository clientRepository,
    IInvoiceRepository invoiceRepository,
    IEngineerRepository engineerRepository,
    IQuarterTariffRepository quarterTariffRepository,
    ISmsService smsService)
    : IRequestHandler<EngAcceptedCommand, string>
{


    public async Task<string> Handle(EngAcceptedCommand request, CancellationToken cancellationToken)
    {
        var client = await clientRepository.GetById(Guid.Parse(currentUser.ClientId));
        if (client is null)
        {
            throw new NotFoundException(nameof(Client), currentUser.ClientId);
        }

        var projectProcess = await electProjectProcessRepository.GetElectProjectProcessById(Guid.Parse(request.EppId));
        if (projectProcess is null) throw new NotFoundException("این تخصیص وجود ندارد");
        if (projectProcess.ProjectLevelEnum == ProjectLevelEnum.ApproveStage)
            throw new NotFoundException("قبلا اعلام نظر کارشناسی انجام شده است");
        if (projectProcess.InspectionStatusEnum is not InspectionStatusEnum.Undefined)
            throw new NotFoundException("قبلا اعلام نظر شده است");

        if (projectProcess.InspectionStatusEnum is InspectionStatusEnum.Canceled)
            throw new NotFoundException("این تخصیص کنسل شده است و  نمیتوان آن را قبول کنید");

        // مشخص کردن کاربر جاری به عنوان کارشناس
        var engineer = await engineerRepository.getByUserId(currentUser.UserId);
        if (engineer is null && !currentUser.Role.Contains("Administrator")) throw new NotFoundException("شما به عنوان کارشناس شناخته نشدید");
        if (projectProcess.Engineer.UserId != currentUser.UserId && !currentUser.Role.Contains("Administrator"))
            throw new NotFoundException("این تخصیص مربوط به شما نمی باشد");

        var quarterTariff = await quarterTariffRepository.GetById(projectProcess.QuarterTariff.Id);
        if (quarterTariff is null) throw new NotFoundException("تخصیص های سه ماهه وجود ندارد");

        var electProject = await electProjectRepository.GetElectProjectById(projectProcess.ElectProject.Id);
        if (electProject is null) throw new NotFoundException("این پرونده وجود ندارد");


        if (projectProcess.ProjectLevelEnum == ProjectLevelEnum.ExpertStage)
        {

            // دریافت فاکتور مربوط به مرحله ایجاد پرونده
            var invoiceCreatedProject = await invoiceRepository.GetInvoiceByProjectId(electProject.Id,
                InvoicePayTypeEnum.CreateProjectStage);
            if (invoiceCreatedProject is null) throw new NotFoundException("فاکتور مرحله کارشناسی ایجاد نشده است");
            if (invoiceCreatedProject.Transaction is null)
                throw new NotFoundException("برای فاکتور مرحله ایجاد پرونده تراکنشی وجود ندارد");

            if (invoiceCreatedProject.ElectProjectProcess is not null)
                throw new NotFoundException("قبلا فاکتور برای کارشناس صادر شده است");

            // اضافه شدن این پروسس به فاکتور
            invoiceCreatedProject.UpdateElectProjectProcess(projectProcess);

            // تغییر این فاکتور به نوع کارشناسی
            invoiceCreatedProject.UpdateInvoicePayType(InvoicePayTypeEnum.ExpertStage);

            projectProcess.UpdateAccepted();

            await electProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        }

        // مرحله تایید مجری ارت
        if (projectProcess.ProjectLevelEnum == ProjectLevelEnum.ErtStage)
        {
            projectProcess.UpdateAccepted();

            await electProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        }

        // تایید کردن مجری تست و تحویل
        if (projectProcess.ProjectLevelEnum == ProjectLevelEnum.TestDeliveryStage)
        {

            // دریافت فاکتور مربوط به مرحله ایجاد پرونده
            var invoiceCreatedProject = await invoiceRepository.GetInvoiceByProjectId(electProject.Id,
                InvoicePayTypeEnum.CreateProjectStage);
            if (invoiceCreatedProject is null) throw new NotFoundException("فاکتور مرحله ایجاد پرونده ایجاد نشده است");
            if (invoiceCreatedProject.Transaction is null)
                throw new NotFoundException("برای فاکتور مرحله ایجاد پرونده تراکنشی وجود ندارد");

            if (invoiceCreatedProject.ElectProjectProcess is not null)
                throw new NotFoundException("قبلا فاکتور برای کارشناس صادر شده است");

            // اساین کردن فاکتور به پروسس
            invoiceCreatedProject.UpdateElectProjectProcess(projectProcess);

            // تغییر این فاکتور به نوع تست و تحویل
            invoiceCreatedProject.UpdateInvoicePayType(InvoicePayTypeEnum.TestAndDelivery);

            projectProcess.UpdateAccepted();

            await electProjectRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }

        await smsService.SendSms3Params(projectProcess.ElectProject.LandlordPhoneNumber, 9808, projectProcess.ElectProject.FileNumber.ToString(),
            engineer?.FullName, Helper.MiladiToShamsiForSms(DateTime.Now.Date));


        return "ok";

    }
}