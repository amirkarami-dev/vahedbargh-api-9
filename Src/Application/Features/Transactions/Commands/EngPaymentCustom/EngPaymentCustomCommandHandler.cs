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
using Coreapi.Domain.AggregatesModel.EngineerAgg;
using Coreapi.Domain.AggregatesModel.ExecutorAgg;
using Coreapi.Domain.AggregatesModel.FinanceAgg;
using MediatR;

namespace Coreapi.Application.Features.Transactions.Commands.EngPaymentCustom
{
    public class EngPaymentCustomCommandHandler:IRequestHandler<EngPaymentCustomCommand,string>
    {
        private readonly ITransactionRepository transactionRepository;
        private readonly IPaymentMelliService paymentMelliService;
        private readonly IClientRepository clientRepository;
        private readonly ISmsService smsService;
        private readonly IUserManager userManager;
        private readonly IEngineerRepository engineerRepository;
        private readonly ICurrentUser currentUser;

        public EngPaymentCustomCommandHandler(ITransactionRepository transactionRepository, IPaymentMelliService paymentMelliService, IClientRepository clientRepository, ISmsService smsService, IUserManager userManager, IEngineerRepository engineerRepository, ICurrentUser currentUser)
        {
            this.transactionRepository = transactionRepository;
            this.paymentMelliService = paymentMelliService;
            this.clientRepository = clientRepository;
            this.smsService = smsService;
            this.userManager = userManager;
            this.engineerRepository = engineerRepository;
            this.currentUser = currentUser;
        }
        public async Task<string> Handle(EngPaymentCustomCommand request, CancellationToken cancellationToken)
        {
            if (request.SolarFishDate.Length != 10) throw new NotFoundException("تاریخ اشتباه می باشد");
            var client = await clientRepository.GetById(Guid.Parse(currentUser.ClientId));
            if (client is null)
            {
                throw new NotFoundException(nameof(Client), currentUser.ClientId);
            }

            var transactionFind = await transactionRepository.GetByBankTransactionId(request.BtId);
            if (transactionFind != null) throw new NotFoundException("این تراکنش تکراری می باشد");

            var engineer = await engineerRepository.GetById(request.EngineerId);
            if (engineer is null) throw new NotFoundException("کارشناس شناسایی نشد");

            var transaction = new Transaction(request.Amount, client, engineer.UserId,
                GatewayTypeEnum.Custom,
                TransactionTypeEnum.Client, request.TransactionStatus, DateTime.Now,
                Helper.MiladiToShamsiFull(DateTime.Now), request.BtId.ToUpper(), "واریز حسابداری:" +
                request.Des + "-" + "به تاریخ:"  + request.SolarFishDate + "-" + "شماره فیش:" + request.FishNumber,null);
            transactionRepository.Add(transaction);
            await transactionRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            //switch (request.TransactionStatus)
            //{
            //    case TransactionStatusEnum.In:
            //        await smsService.SendSmsCode(engineer.CellPhone, 4662, request.Amount.ToString("N0"),
            //            request.SolarFishDate);
            //        break;
            //    case TransactionStatusEnum.Out:
            //        await smsService.SendSmsCode(engineer.CellPhone, 3464, request.Amount.ToString("N0"),
            //            "برداشت حسابداری واحد برق");
            //        break;
            //}
            return transaction.Id.ToString();
        }
    }
}
