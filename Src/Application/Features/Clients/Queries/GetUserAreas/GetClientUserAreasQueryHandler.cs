using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Domain.AggregatesModel.ClientAggregate;

namespace Coreapi.Application.Features.Clients.Queries.GetUserAreas;

public class GetClientUserAreasQueryHandler : IRequestHandler<GetClientUserAreasQuery, IEnumerable<UserAreaDto>>
{
    private readonly IClientRepository repository;
    private readonly ICurrentUser currentUser;
    private readonly IMapper mapper;

    public GetClientUserAreasQueryHandler(IClientRepository repository, ICurrentUser currentUser, IMapper mapper)
    {
        this.repository = repository;
        this.currentUser = currentUser;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<UserAreaDto>> Handle(GetClientUserAreasQuery request, CancellationToken cancellationToken)
    {
        if(!currentUser.Role.Equals("Administrator"))
            request.UserId = currentUser.UserId;

        var areas = await repository.GetUserAreas(Guid.Parse(currentUser.ClientId), request.UserId);

        return mapper.Map<IEnumerable<UserAreaDto>>(areas);
    }
}
