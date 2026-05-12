using System;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Common.Enums;
using Coreapi.Common.Utility;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.EngineerAgg;
using Coreapi.Domain.AggregatesModel.FinanceAgg;
using MediatR;

namespace Coreapi.Application.Features.Transactions.Commands.EngPaymentApproved;

public class EngPaymentApprovedCommandHandler:IRequestHandler<EngPaymentApprovedCommand,string>
{
    private readonly IEngPaymentTaskRepository engPaymentTaskRepository;
    private readonly IEngPaymentListRepository engPaymentListRepository;
    private readonly IClientRepository clientRepository;
    private readonly ICurrentUser currentUser;
    private readonly ITransactionRepository transactionRepository;
    private readonly IEngineerRepository engineerRepository;

    public EngPaymentApprovedCommandHandler(IEngPaymentTaskRepository engPaymentTaskRepository, IEngPaymentListRepository engPaymentListRepository, IClientRepository clientRepository, ICurrentUser currentUser, ITransactionRepository transactionRepository, IEngineerRepository engineerRepository)
    {
        this.engPaymentTaskRepository = engPaymentTaskRepository;
        this.engPaymentListRepository = engPaymentListRepository;
        this.clientRepository = clientRepository;
        this.currentUser = currentUser;
        this.transactionRepository = transactionRepository;
        this.engineerRepository = engineerRepository;
    }
    public async Task<string> Handle(EngPaymentApprovedCommand request, CancellationToken cancellationToken)
    {
        var client = await clientRepository.GetById(Guid.Parse(currentUser.ClientId));
        if (client is null)
            throw new NotFoundException(nameof(Client), currentUser.ClientId);

        var engPaymentTask = await engPaymentTaskRepository.GetById(Guid.Parse(request.EngPaymentTaskId));
        if (engPaymentTask is null) throw new NotFoundException("تسک پرداخت وجود ندارد");

        if (engPaymentTask.Approved) throw new NotFoundException("این لیست قبلا پرداخت شده است");

        var engPaymentList = await engPaymentListRepository.GetEngPaymentList(engPaymentTask.Id);

        if (engPaymentList is null) throw new NotFoundException("لیست پرداخت وجود ندارد");

        foreach (var paymentList in engPaymentList)
        {
            if (paymentList.Transaction != null && paymentList.Engineer == null)
                throw new NotFoundException("خطا در لیست تراکنش یا لیست کارشناس");
          
            var transaction = new Transaction(paymentList.SumAmountWithFish, client, paymentList.Engineer.UserId,
                GatewayTypeEnum.Custom,
                TransactionTypeEnum.Client, TransactionStatusEnum.In, engPaymentTask.JulianCreated,
                engPaymentTask.SolarCreated, paymentList.Engineer.UserId.ToUpper() + "-"+ engPaymentTask.SolarCreated, "واریز حسابداری:" +
                engPaymentTask.Des + "-" + "به تاریخ:" + request.SolarApproved + "-" + "دوره پرداخت:" + engPaymentTask.Period,null);
            transactionRepository.Add(transaction);

            paymentList.UpdateTransaction(transaction);

        }

        engPaymentTask.PayTask(request.SolarApproved, Helper.ShamsiToMiladi(request.SolarApproved));

        await engPaymentTaskRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return engPaymentTask.Id.ToString();

    }
}