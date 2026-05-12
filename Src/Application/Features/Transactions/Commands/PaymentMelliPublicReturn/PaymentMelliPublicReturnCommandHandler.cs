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
using Coreapi.Domain.AggregatesModel.FinanceAgg;
using MediatR;

namespace Coreapi.Application.Features.Transactions.Commands.PaymentMelliPublicReturn
{
    // این کلاس برای شارژ توسط مالک هستش که پیامک ارسال میشه
    public class PaymentMelliPublicReturnCommandHandler(
        IBankTransactionRepository bankTransactionRepository,
        ITransactionRepository transactionRepository,
        IPaymentMelliService paymentMelliService,
        ISmsService smsService,
        IElectProjectRepository electProjectRepository,
        IInvoiceRepository invoiceRepository)
        : IRequestHandler<PaymentMelliPublicReturnCommand, string>
    {
        private static readonly SemaphoreSlim _transactionLock = new(1, 1);

        public async Task<string> Handle(PaymentMelliPublicReturnCommand request, CancellationToken cancellationToken)
        {
            await _transactionLock.WaitAsync(cancellationToken); // گرفتن قفل
            var callBackUrl = paymentMelliService.GetVerifyCallBackPublicUrl();

            try
            {
                if (Convert.ToInt32(request.ResponseCode) != 0) return callBackUrl + "?result=" + "error";


                var bankTransaction =
                    await bankTransactionRepository.GetByPaymentIdAndToken(request.PaymentId, request.Token);
                if (bankTransaction == null) return callBackUrl + "?result=error-transaction-found";


                if (bankTransaction.Confirm)
                    return callBackUrl + "?result=ok-" + bankTransaction.SystemTraceAuditNumber;


                var verifyResult = await paymentMelliService.VerifyPaymentMelli(request.RetrievalReferenceNumber,
                    request.SystemTraceAuditNumber, request.Token);

                if (!verifyResult.status) return callBackUrl + "?result=error-transaction-verify";



                bankTransaction.UpdateWhenReturn(
                    request.AcceptorId, request.RequestId, request.RetrievalReferenceNumber,
                    request.SystemTraceAuditNumber, request.MaskedPan, request.Sha256OfPan, verifyResult.description
                );

                await bankTransactionRepository.UnitOfWork.SaveChangesAsync(cancellationToken);


                var getTransaction = await transactionRepository.GetByBankTransactionId(bankTransaction.Id.ToString());
                if (getTransaction is not null)
                {
                    if (getTransaction.Amount == Convert.ToInt64(verifyResult.result.amount))
                        return callBackUrl + "?result=" + "error-transaction-verify";
                }

                // دریافت پرونده
                var electProject =
                    await electProjectRepository.GetElectProjectById(Guid.Parse(bankTransaction.ProjectId));

                if (electProject.IsBigProject)
                {
                    if (electProject.AmountPerArea > Convert.ToInt64(verifyResult.result.amount))
                        throw new NotFiniteNumberException("مبلغ وارده برای زیرپروزه ها کافی نمی باشد");
                    foreach (var childProject in electProject.ChildProjects)
                    {

                        if (childProject.IsBuildingInspection)
                        {
                            var invoiceExpert = await invoiceRepository.GetInvoiceByProjectId(childProject.Id,
                                InvoicePayTypeEnum.CreateProjectStage);
                            var invoiceService = await invoiceRepository.GetInvoiceByProjectId(childProject.Id,
								InvoicePayTypeEnum.ProjectServices);
							if (invoiceExpert is null || invoiceService is null)
                                throw new NotFoundException(
                                    $"برای این پرونده فاکتور ایجاد پرونده/خدمات ثبت نشده است: {childProject.FileNumber}");

                            var transactionInspectionChild = new Transaction(
                                invoiceExpert.Amount + invoiceService.Amount,
                                electProject.Client,
                                electProject.Client.Id.ToString(),
                                GatewayTypeEnum.IranKish,
                                TransactionTypeEnum.Client,
                                TransactionStatusEnum.In, DateTime.Now,
                                Helper.MiladiToShamsiFull(DateTime.Now),
                                bankTransaction.Id + "-" + childProject.FileNumber,
                                verifyResult.description + "-" + "کارشناسی" + "-" + "کدرهگیری:" +
                                request.SystemTraceAuditNumber + "-" + $"پرونده اصلی:{electProject.FileNumber}"
                                + "-" + $"مبلغ تراکنش اصلی:{electProject.AmountPerArea}",
                                childProject.Id.ToString()
                            );

                            transactionRepository.Add(transactionInspectionChild);
                        }

                        if (!childProject.IsEarthSystem) continue;
                        {
                            var invoiceErt = await invoiceRepository.GetInvoiceByProjectId(childProject.Id,
                                InvoicePayTypeEnum.NezamStage);
                            if (invoiceErt is null)
                                throw new NotFoundException(
                                    $"برای این پرونده فاکتور ارت ثبت نشده است: {childProject.FileNumber}");
                            var transactionErtChild = new Transaction(
                                invoiceErt.Amount,
                                electProject.Client,
                                electProject.Client.Id.ToString(),
                                GatewayTypeEnum.IranKish,
                                TransactionTypeEnum.Client,
                                TransactionStatusEnum.In,
                                DateTime.Now,
                                Helper.MiladiToShamsiFull(DateTime.Now),
                                bankTransaction.Id + "-" + childProject.FileNumber,
                                verifyResult.description + "-" + "ارت" + "-" + "کدرهگیری:" +
                                request.SystemTraceAuditNumber + "-" + $"پرونده اصلی:{electProject.FileNumber}"
                                + "-" + $"مبلغ تراکنش اصلی:{electProject.AmountPerArea}",
                                childProject.Id.ToString()
                            );

                            transactionRepository.Add(transactionErtChild);
                        }

                    }
                }

                else
                {


                    var transaction = new Transaction(Convert.ToInt64(verifyResult.result.amount),
                        bankTransaction.Client,
                        bankTransaction.UserId,
                        GatewayTypeEnum.IranKish,
                        TransactionTypeEnum.Client, TransactionStatusEnum.In, DateTime.Now.Date,
                        Helper.MiladiToShamsiFull(DateTime.Now.Date), bankTransaction.Id.ToString(),
                        verifyResult.description + "-" + "کدرهگیری:" + request.SystemTraceAuditNumber,
                        bankTransaction.ProjectId);

                    transactionRepository.Add(transaction);

                }



                await transactionRepository.UnitOfWork.SaveChangesAsync(cancellationToken);



                if (electProject != null)
                    await smsService.SendSms4Params(electProject.LandlordPhoneNumber, 10443,
                        electProject.FileNumber.ToString(), Helper.MiladiToShamsiForSms(DateTime.Now.Date),
                        Convert.ToInt64(verifyResult.result.amount).ToString("N0"), request.SystemTraceAuditNumber);

                return callBackUrl + "?result=ok-" + bankTransaction.SystemTraceAuditNumber;
            }
            catch (Exception e)
            {
                await smsService.SendSms4Params("09120833933", 10443,
                    "مشکل در انجام تراکنش", Helper.MiladiToShamsiForSms(DateTime.Now.Date),
                  e.Message, request.SystemTraceAuditNumber);
                return callBackUrl + "?result=error-transaction-verify";
            }
            finally
            {
                    _transactionLock.Release(); // آزاد کردن قفل
            }


        }

    }

}
