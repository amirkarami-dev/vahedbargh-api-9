using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Common.Enums;
using Coreapi.Common.Utility;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.FinanceAgg;
using MediatR;

namespace Coreapi.Application.Features.Transactions.Queries.GetEngPayments;

public class GetEngPaymentQueryHandler:IRequestHandler<GetEngPaymentQuery,IEnumerable<EngPaymentDto>>
{
    private readonly IEngPaymentListRepository engPaymentListRepository;
    private readonly IEngPaymentTaskRepository engPaymentTaskRepository;
    private readonly IClientRepository clientRepository;
    private readonly ICurrentUser currentUser;
    private readonly ITransactionRepository transactionRepository;
    private readonly IMapper mapper;

    public GetEngPaymentQueryHandler(IEngPaymentListRepository engPaymentListRepository, IEngPaymentTaskRepository engPaymentTaskRepository, IClientRepository clientRepository, ICurrentUser currentUser, ITransactionRepository transactionRepository, IMapper mapper)
    {
        this.engPaymentListRepository = engPaymentListRepository;
        this.engPaymentTaskRepository = engPaymentTaskRepository;
        this.clientRepository = clientRepository;
        this.currentUser = currentUser;
        this.transactionRepository = transactionRepository;
        this.mapper = mapper;
    }
    public async Task<IEnumerable<EngPaymentDto>> Handle(GetEngPaymentQuery request, CancellationToken cancellationToken)
    {
        var client = await clientRepository.GetById(Guid.Parse(currentUser.ClientId));
        if (client is null)
            throw new NotFoundException(nameof(Client), currentUser.ClientId);

        if (string.IsNullOrEmpty(request.EngPaymentTaskId) || request.EngPaymentTaskId == "null") throw new NotFoundException("لیست پرداخت وجود ندارد");

   

        var engPaymentTask = await engPaymentTaskRepository.GetById(Guid.Parse(request.EngPaymentTaskId));
        if (engPaymentTask is null) throw new NotFoundException("این دوره پرداخت یافت نشد");

        var invoices = await transactionRepository.GetClientEngPaymentByEng(client.Id, Helper.ShamsiToMiladi(engPaymentTask.FromSolar), Helper.ShamsiToMiladi(engPaymentTask.ToSolar));
        var transactionsIns = await transactionRepository.GetClientTransactions(client.Id, TransactionStatusEnum.In);
        var engPaymentList = await engPaymentListRepository.GetEngPaymentList(engPaymentTask.Id);

        var result = mapper.Map<IEnumerable<EngPaymentDto>>(engPaymentList);
        foreach (var dto in result)
        {
            var resultInvoices = invoices.Where(w => w.ElectProjectProcess.Engineer.Id == dto.Engineer.Id).ToList();
            dto.Invoices = resultInvoices;
            dto.TransactionIn = transactionsIns.Where(i=> !string.IsNullOrEmpty(i.ProjectId) && resultInvoices.Any(inv=>inv.ElectProject.Id == Guid.Parse(i.ProjectId))).ToList();
        }

        return result;
        

    }
}