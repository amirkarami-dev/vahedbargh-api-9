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
using Coreapi.Application.Common.Models;
using Coreapi.Common.Utility;
using Coreapi.Domain.AggregatesModel.ClientAggregate;

namespace Coreapi.Application.Features.Clients.Queries.GetAreas;

public class GetClientAreasQueryHandler : IRequestHandler<GetClientAreasQuery, IEnumerable<ClientAreaDto>>
{
    private readonly IClientRepository repository;
    private readonly ICurrentUser currentUser;
    private readonly IUserManager userManager;

    public GetClientAreasQueryHandler(IClientRepository repository, ICurrentUser currentUser, IUserManager userManager)
    {
        this.repository = repository;
        this.currentUser = currentUser;
        this.userManager = userManager;
    }

    public async Task<IEnumerable<ClientAreaDto>> Handle(GetClientAreasQuery request, CancellationToken cancellationToken)
    {
        var client = await repository.GetWithAreas(Guid.Parse(currentUser.ClientId));
        if(client is null)
            throw new NotFoundException(nameof(Client), currentUser.ClientId);

        var userIds = client.Areas.Select(t => t.Users.Select(tu => tu.UserId)).AsOneList();

        List<UserData> users = new();
        if (userIds.Any())
            users = await userManager.GetUsers(userIds);

        return client.Areas.Select(area => new ClientAreaDto
        {
            Id = area.Id,
            Name = area.Name,
            Description = area.Description,
            Radius = area.Radius,
            Type = area.Type,
            Points = area.Points.Select(cp => new ClientAreaPointDto { Latitude = cp.Location.Latitude, Longitude = cp.Location.Longitude }),
            Users = users.Where(u => area.Users.Any(au => au.UserId.Equals(u.Id))).Select(u => new ClientAreaUserDto { Id = u.Id, FirstName = u.FirstName, LastName = u.LastName, Avatar = u.Avatar })
        });

    }
}
