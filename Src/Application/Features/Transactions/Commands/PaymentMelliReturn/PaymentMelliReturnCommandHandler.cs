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

namespace Coreapi.Application.Features.Transactions.Commands.PaymentMelliReturn
{
    public class PaymentMelliReturnCommandHandler(
        IBankTransactionRepository bankTransactionRepository,
        ITransactionRepository transactionRepository,
        IPaymentMelliService paymentMelliService,
        ISmsService smsService,
        IUserManager userManager,
        IElectProjectRepository electProjectRepository,
        IInvoiceRepository invoiceRepository
        
        )
        : IRequestHandler<PaymentMelliReturnCommand, string>
    {

        public async Task<string> Handle(PaymentMelliReturnCommand request, CancellationToken cancellationToken)
        {
            var callBackUrl = paymentMelliService.GetVerifyCallBackPublicUrl();
            if (Convert.ToInt32(request.ResponseCode) != 0) return callBackUrl + "?result=" + "error";

            var bankTransaction = await bankTransactionRepository.GetByPaymentIdAndToken(request.PaymentId, request.Token);
            if (bankTransaction == null) return callBackUrl + "?result=error-transaction-found";


            if (bankTransaction.Confirm)
                throw new NotFoundException("این تراکنش قبلا تایید شده است");

            bankTransaction.UpdateWhenReturn(
                request.AcceptorId, request.RequestId, request.RetrievalReferenceNumber,
                request.SystemTraceAuditNumber, request.MaskedPan, request.Sha256OfPan, "در انتظار تایید"
            );

            var verifyResult = await paymentMelliService.VerifyPaymentMelli(request.RetrievalReferenceNumber, request.SystemTraceAuditNumber, request.Token);

            if (!verifyResult.status) return callBackUrl + "?result=error-transaction-verify";

            await bankTransactionRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            var electProject = await electProjectRepository.GetElectProjectById(Guid.Parse(bankTransaction.ProjectId));

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
                        if (invoiceExpert is null)
                            throw new NotFoundException(
                                $"برای این پرونده فاکتور ایجاد پرونده ثبت نشده است: {childProject.FileNumber}");
                        var transactionInspectionChild = new Transaction(
                            invoiceExpert.Amount, electProject.Client, 
                            electProject.Client.Id.ToString(),
                            GatewayTypeEnum.Custom,
                            TransactionTypeEnum.Client, TransactionStatusEnum.In, DateTime.Now,
                            Helper.MiladiToShamsiFull(DateTime.Now),
                            bankTransaction.SystemTraceAuditNumber + "-" + childProject.FileNumber,
                            verifyResult.description + "-" + "توسط مالک" + "-" + $"Paren:{electProject.FileNumber}",
                            childProject.Id.ToString());

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
                            invoiceErt.Amount, electProject.Client, 
                            electProject.Client.Id.ToString(),
                            GatewayTypeEnum.Custom,
                            TransactionTypeEnum.Client, TransactionStatusEnum.In,
                            DateTime.Now,
                            Helper.MiladiToShamsiFull(DateTime.Now),
                            bankTransaction.SystemTraceAuditNumber + "-" + childProject.FileNumber,
                            verifyResult.description + "-" + "توسط مالک" + "-" + $"Paren:{electProject.FileNumber}",
                            childProject.Id.ToString()
                            );

                        transactionRepository.Add(transactionErtChild);
                    }

                }
            }

            else
            {
                var transaction = new Transaction(
                    Convert.ToInt64(verifyResult.result.amount), 
                    bankTransaction.Client, bankTransaction.UserId,
                    GatewayTypeEnum.IranKish,
                    TransactionTypeEnum.Client, TransactionStatusEnum.In, DateTime.Now,
                    Helper.MiladiToShamsiFull(DateTime.Now),
                    bankTransaction.SystemTraceAuditNumber, 
                    verifyResult.description + "-" + "توسط مالک", 
                    bankTransaction.ProjectId);

                transactionRepository.Add(transaction);
            }





            await transactionRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            var user = await userManager.GetUserAsync(bankTransaction.UserId);
            if (user != null) await smsService.SendSmsCode(user.PhoneNumber, 899, Convert.ToInt64(verifyResult.result.amount).ToString("N0"), "رهگیری:" + bankTransaction.SystemTraceAuditNumber + "واحدبرق" + "توسط مالک");

            return callBackUrl + "?result=ok-" + bankTransaction.SystemTraceAuditNumber;


        }
    }
}
