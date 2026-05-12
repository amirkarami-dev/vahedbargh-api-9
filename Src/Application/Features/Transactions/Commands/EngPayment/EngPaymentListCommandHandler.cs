using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Common.Utility;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.EngineerAgg;
using Coreapi.Domain.AggregatesModel.FinanceAgg;
using MediatR;

namespace Coreapi.Application.Features.Transactions.Commands.EngPayment;

public class EngPaymentListCommandHandler(
    ITransactionRepository transactionRepository,
    ICurrentUser currentUser,
    IClientRepository clientRepository,
    IEngineerRepository engineerRepository,
    IEngPaymentListRepository engPaymentListRepository,
    IEngPaymentTaskRepository engPaymentTaskRepository
    )
    : IRequestHandler<EngPaymentListCommand, string>
{



    public async Task<string> Handle(EngPaymentListCommand request, CancellationToken cancellationToken)
    {
        var client = await clientRepository.GetById(Guid.Parse(currentUser.ClientId));
        if (client is null)
            throw new NotFoundException(nameof(Client), currentUser.ClientId);

        if ( Helper.ShamsiToMiladi(request.FromSolar) > Helper.ShamsiToMiladi(request.ToSolar))
            throw new NotAllowedException("تاریخ شروع نباید بزرگتر تاریخ پایان باشد");
        var transactions = await transactionRepository.GetClientEngPaymentGroup(client.Id, Helper.ShamsiToMiladi(request.FromSolar), Helper.ShamsiToMiladi(request.ToSolar));

        if (transactions is null) throw new NotFoundException("در تاریخ انتخابی تراکنشی پرداختی وجود ندارد");

        var getLatestPeriod = await engPaymentTaskRepository.GetLatestPeriod()+1;
        var engineers = await engineerRepository.GetAll();
        var listEngineer = engineers.ToList();

        var engPaymentTask = new EngPaymentTask(client, Helper.MiladiToShamsi(DateTime.Now), DateTime.Now,
            getLatestPeriod,
            request.FromSolar, Helper.ShamsiToMiladi(request.FromSolar), request.ToSolar,
            Helper.ShamsiToMiladi(request.ToSolar), request.Description, null);

        foreach (var transaction in transactions)
        {
            var engineer = listEngineer.FirstOrDefault(w => w.Id == transaction.EngineerId);
            if (engineer is null) throw new NotFoundException("در یکی از تراکنش ها مهندس مشخص نشده است");

            var engPayment = new EngPaymentList(engineer, engPaymentTask,
                  transaction.Amount,engineer.BankAccountBlocked);
            engPaymentListRepository.Add(engPayment);
        }

        await engPaymentListRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return engPaymentTask.Id.ToString();
    }
}