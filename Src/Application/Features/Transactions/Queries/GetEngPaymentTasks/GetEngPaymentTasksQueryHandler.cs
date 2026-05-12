using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.FinanceAgg;
using MediatR;

namespace Coreapi.Application.Features.Transactions.Queries.GetEngPaymentTasks;

public class GetEngPaymentTasksQueryHandler:IRequestHandler<GetEngPaymentTasksQuery,IEnumerable<EngPaymentTaskDto>>
{
    private readonly IEngPaymentTaskRepository engPaymentTaskRepository;
    private readonly IClientRepository clientRepository;
    private readonly ICurrentUser currentUser;
    private readonly IMapper mapper;


    public GetEngPaymentTasksQueryHandler(IEngPaymentTaskRepository engPaymentTaskRepository, IClientRepository clientRepository, ICurrentUser currentUser, IMapper mapper)
    {
        this.engPaymentTaskRepository = engPaymentTaskRepository;
        this.clientRepository = clientRepository;
        this.currentUser = currentUser;
        this.mapper = mapper;
    }
    public async Task<IEnumerable<EngPaymentTaskDto>> Handle(GetEngPaymentTasksQuery request, CancellationToken cancellationToken)
    {
        var client = await clientRepository.GetById(Guid.Parse(currentUser.ClientId));
        if (client is null)
            throw new NotFoundException(nameof(Client), currentUser.ClientId);

        var engPaymentTasks = await engPaymentTaskRepository.GetByOrderPeriod();
        if (engPaymentTasks is null) throw new NotFoundException("لیست پرداخت ندارد");

        return mapper.Map<IEnumerable<EngPaymentTaskDto>>(engPaymentTasks);

    }
}