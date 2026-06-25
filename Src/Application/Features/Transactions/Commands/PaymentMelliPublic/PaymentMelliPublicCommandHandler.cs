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

namespace Coreapi.Application.Features.Transactions.Commands.PaymentMelliPublic
{
    public class PaymentMelliPublicCommandHandler(
        IBankTransactionRepository bankTransactionRepository,
        IClientRepository clientRepository,
        IPaymentMelliService paymentMelliService,
        ITransactionRepository transactionRepository,
        IElectProjectRepository electProjectRepository)
        : IRequestHandler<PaymentMelliPublicCommand, string>
    {

        public async Task<string> Handle(PaymentMelliPublicCommand request, CancellationToken cancellationToken)
        {
            var random = new Random();

            var client = await clientRepository.GetById(Guid.Parse("d2aa968b-b169-492d-8493-36456f094e07"));
            if (client is null)
            {
                throw new NotAllowedException("این پرداختی مربوط به واحد برق نمی باشد");
            }

            var decodeId = Helper.DecodeGuid(request.ElectProjectId);

            var electProject = await electProjectRepository.GetElectProjectById(decodeId);
            if (electProject is null) throw new NotFoundException("پرونده وجود ندارد");

            // غیر فعال کردن موقت برای پرداخت 146 هزاری ارت
            // if(electProject.ParentProject is not null) throw new NotFoundException("برای زیر پروژه پرداختی قابل انجام نیست");

			var getTransaction = await transactionRepository.GetByElectProjectId(decodeId.ToString(),TransactionStatusEnum.In);
			var amount = electProject.IsBigProject ? await transactionRepository.GetBigProjectBalance([.. electProject.ChildProjects.Select(s => s.Id.ToString())]) : await transactionRepository.GetProjectBalance(electProject.Id);
            var amountForPay = Math.Abs(amount);

			if (amount >= 0 )
                throw new NotFoundException("بالانس پرونده صفر می باشد و پرداخت قابل انجام نیست");

            if (getTransaction != null && getTransaction.Amount == amount && !electProject.IsBigProject) 
                throw new NotFoundException("برای این شماره پرونده پرداختی انجام شده است");

            if (electProject.IsBigProject)
            {
                if (electProject.ChildProjects == null) throw new NotFoundException("مشکل در زیر پرونده ها");
                var childIds = electProject.ChildProjects.Select(s => s.Id.ToString()).ToList();
                var hasChildTrans =
                    await transactionRepository.HasTransactionInForChild(childIds, TransactionStatusEnum.In);
                if (hasChildTrans) throw new NotFoundException("برای این پرونده قبلا پرداختی وجود دارد");
			}

            var paymentId = random.Next(1,100).ToString() + electProject.FileNumber;

            var resultForGetToken = await paymentMelliService.GetPaymentMelliPublicUrl(amountForPay, paymentId);
            if (!resultForGetToken.status)
            {
                var errorText = Helper.ReturnIranKishError(resultForGetToken.responseCode);
                throw new NotFoundException(errorText);
            }

            var bankTransaction = new BankTransaction(GatewayTypeEnum.Melli, client, client.Id.ToString(),
                PaymentTypeEnum.Online, resultForGetToken.result.token, paymentId,electProject.Id.ToString(), amountForPay,
                Helper.MiladiToShamsiFull(DateTime.Now.Date));


            bankTransactionRepository.Add(bankTransaction);
            await bankTransactionRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return $"https://ikc.shaparak.ir/iuiv3/IPG/Index?token={resultForGetToken.result.token}";
            
        }
    }
}
