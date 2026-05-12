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

namespace Coreapi.Application.Features.Clients.Commands.AssignAreaUsers;

public class AssignClientAreaUsersCommandHandler : IRequestHandler<AssignClientAreaUsersCommand>
{
    private readonly IClientRepository repository;
    private readonly ICurrentUser currentUser;
    private readonly IUserManager userManager;

    public AssignClientAreaUsersCommandHandler(IClientRepository repository, ICurrentUser currentUser, IUserManager userManager)
    {
        this.repository = repository;
        this.currentUser = currentUser;
        this.userManager = userManager;
    }

    public async Task Handle(AssignClientAreaUsersCommand request, CancellationToken cancellationToken)
    {
        var client = await repository.GetWithAreas(Guid.Parse(currentUser.ClientId));
        if (client is null)
            throw new NotFoundException(nameof(Client), currentUser.ClientId);

        ClientArea area = client.Areas.FirstOrDefault(a => a.Id == request.AreaId);
        if (area is null)
            throw new NotFoundException(nameof(ClientArea), request.AreaId);

        var users = await userManager.GetClientUsersByAdministrator(currentUser.UserId, request.Users);

        area.Users.Where(a => !users.Any(u => a.UserId.Equals(u.Id))).ToList().ForEach(a => area.RemoveUser(a.UserId));

        users.Where(a => !area.Users.Any(au => au.UserId.Equals(a.Id))).ToList().ForEach(a => area.AddUser(a.Id));

        await repository.UnitOfWork.SaveChangesAsync(cancellationToken);

    }
}
