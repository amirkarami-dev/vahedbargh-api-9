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

namespace Coreapi.Application.Features.Transactions.Commands.PaymentMelli
{
    public class PaymentMelliCommandHandler(
        IBankTransactionRepository bankTransactionRepository,
        ICurrentUser currentUser,
        IClientRepository clientRepository,
        IPaymentMelliService paymentMelliService,
        IElectProjectRepository electProjectRepository)
        : IRequestHandler<PaymentMelliCommand, string>
    {

        public async Task<string> Handle(PaymentMelliCommand request, CancellationToken cancellationToken)
        {
            var random = new Random();
            var dd = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            
            var client = await clientRepository.GetById(Guid.Parse(currentUser.ClientId));
            if (client is null)
            {
                throw new NotFoundException(nameof(Client), currentUser.ClientId);
            }

            var electProject = await electProjectRepository.GetById(Guid.Parse(request.ElectProjectId));
            if (electProject is null) throw new NotFoundException("پرونده وجود ندارد");

            var paymentId = random.Next(1, 100).ToString() + electProject.FileNumber;

            var resultForGetToken = await paymentMelliService.GetPaymentMelliUrl(request.Amount, paymentId);

           var bankTransaction = new BankTransaction(GatewayTypeEnum.Melli, client, client.Id.ToString(),
               PaymentTypeEnum.Online, resultForGetToken.result.token, paymentId, electProject.Id.ToString(), request.Amount,
               Helper.MiladiToShamsiFull(DateTime.Now.Date));


            bankTransactionRepository.Add(bankTransaction);
                   await bankTransactionRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

                   return $"https://sadad.shaparak.ir/Purchase/Index?token={resultForGetToken.result.token}";

        }
    }
}
