using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Common.Enums;
using Coreapi.Common.Utility;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using Coreapi.Domain.AggregatesModel.ExecutorAgg;
using Coreapi.Domain.AggregatesModel.FinanceAgg;
using MediatR;

namespace Coreapi.Application.Features.Transactions.Commands.PaymentCustom
{
    public class PaymentCustomCommandHandler(
        ITransactionRepository transactionRepository,
        IPaymentMelliService paymentMelliService,
        IElectProjectRepository electProjectRepository,
        IClientRepository clientRepository,
        ISmsService smsService,
        IUserManager userManager,
        ICurrentUser currentUser,
        IInvoiceRepository invoiceRepository)
        : IRequestHandler<PaymentCustomCommand, string>
    {
        private readonly IPaymentMelliService paymentMelliService = paymentMelliService;
        private readonly IUserManager userManager = userManager;

        public async Task<string> Handle(PaymentCustomCommand request, CancellationToken cancellationToken)
        {
            // throw new NotFoundException("پرداخت به صورت فیش غیر فعال می باشد ");
        
                if (request.SolarFishDate.Length != 10) throw new NotFoundException("تاریخ اشتباه می باشد");
                var client = await clientRepository.GetById(Guid.Parse(currentUser.ClientId));
                if (client is null)
                {
                    throw new NotFoundException(nameof(Client), currentUser.ClientId);
                }

                var transactionFind = await transactionRepository.GetByBankTransactionId(request.BtId);
                if (transactionFind != null) throw new NotFoundException("این شماره پیگیری تکراری می باشد");

                var electProject = await electProjectRepository.GetElectProjectById(request.ElectProjectId);
                if (electProject is null) throw new NotFoundException("این پرونده وجود ندارد");
                if (electProject.IsStop) throw new NotFoundException("برای پرونده متوقف شده نمیتوان تراکنش ثبت کرد");

                if (electProject.IsBigProject)
                {
                    if (electProject.AmountPerArea > request.Amount)
                        throw new NotFiniteNumberException("مبلغ وارده برای زیرپروزه ها کافی نمی باشد");
                    foreach (var childProject in electProject.ChildProjects)
                    {
                      
                        if (childProject.IsBuildingInspection)
                        {
                            var invoiceExpert = await invoiceRepository.GetInvoiceByProjectId(childProject.Id,
                                InvoicePayTypeEnum.CreateProjectStage);
                            if (invoiceExpert is null)
                                throw new NotFoundException(
                                    $"برای این پرونده فاکتور ایجاد پرونده ثبت نشده است: {childProject.FileNumber}");
                            var transactionInspectionChild = new Transaction(invoiceExpert.Amount, client, client.Id.ToString(),
                                GatewayTypeEnum.Custom,
                                TransactionTypeEnum.Client, request.TransactionStatus, DateTime.Now,
                                Helper.MiladiToShamsiFull(DateTime.Now), request.BtId.ToUpper() + "-" + childProject.FileNumber, "حسابداری:" +
                                request.Des + "-" + request.SolarFishDate + "-" + request.FishNumber + "شماره فیش" + "-" + $"Paren:{electProject.FileNumber}", childProject.Id.ToString());

                            transactionRepository.Add(transactionInspectionChild);
                        }

                        if (!childProject.IsEarthSystem) continue;
                        {
                            var invoiceErt = await invoiceRepository.GetInvoiceByProjectId(childProject.Id,
                                InvoicePayTypeEnum.NezamStage);
                            if (invoiceErt is null)
                                throw new NotFoundException(
                                    $"برای این پرونده فاکتور ارت ثبت نشده است: {childProject.FileNumber}");
                            var transactionErtChild = new Transaction(invoiceErt.Amount, client, client.Id.ToString(),
                                GatewayTypeEnum.Custom,
                                TransactionTypeEnum.Client, request.TransactionStatus, DateTime.Now,
                                Helper.MiladiToShamsiFull(DateTime.Now), request.BtId.ToUpper() + "-" + childProject.FileNumber, "حسابداری:" +
                                request.Des + "-" + request.SolarFishDate + "-" + request.FishNumber + "شماره فیش" + "-" + $"Paren:{electProject.FileNumber}", childProject.Id.ToString());

                            transactionRepository.Add(transactionErtChild);
                        }

                    }
                }
                else
                {

                    var transaction = new Transaction(request.Amount, client, client.Id.ToString(),
                    GatewayTypeEnum.Custom,
                    TransactionTypeEnum.Client, request.TransactionStatus, DateTime.Now,
                    Helper.MiladiToShamsiFull(DateTime.Now), request.BtId.ToUpper(), "حسابداری:" +
                    request.Des + "-" + request.SolarFishDate + "-" + request.FishNumber + "شماره فیش", electProject.Id.ToString());

                    transactionRepository.Add(transaction);
                }



                await transactionRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

                switch (request.TransactionStatus)
                {
                    case TransactionStatusEnum.In:
                        await smsService.SendSms3Params(electProject.LandlordPhoneNumber, 8867, electProject.FileNumber.ToString(), request.Amount.ToString("N0"),
                            "تاریخ:"+ Helper.MiladiToShamsiForSms(DateTime.Now.Date));
                        break;
                    case TransactionStatusEnum.Out:
                        await smsService.SendSms3Params(electProject.LandlordPhoneNumber, 8868, electProject.FileNumber.ToString(), request.Amount.ToString("N0"),
                            "تاریخ:"+ Helper.MiladiToShamsiForSms(DateTime.Now.Date));
                        break;
                }

                return "ok";
            }
    }
}
