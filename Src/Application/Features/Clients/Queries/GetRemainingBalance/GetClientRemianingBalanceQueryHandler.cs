using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Domain.AggregatesModel.ClientAggregate;

namespace Coreapi.Application.Features.Clients.Queries.GetRemainingBalance;

public class GetClientRemianingBalanceQueryHandler : IRequestHandler<GetClientRemainingBalanceQuery, RemainingBalanceDto>
{
    private readonly IClientRepository repository;
    private readonly ICurrentUser currentUser;
    private readonly IMapper mapper;

    public GetClientRemianingBalanceQueryHandler(IClientRepository repository, ICurrentUser currentUser, IMapper mapper)
    {
        this.repository = repository;
        this.currentUser = currentUser;
        this.mapper = mapper;
    }

    public async Task<RemainingBalanceDto> Handle(GetClientRemainingBalanceQuery request, CancellationToken cancellationToken)
    {
        var client = await repository.GetById(Guid.Parse(currentUser.ClientId));
        if (client is null)
            throw new NotFoundException(nameof(Client), currentUser.ClientId);

        return mapper.Map<RemainingBalanceDto>(client);
    }
}
