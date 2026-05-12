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

namespace Coreapi.Application.Features.Clients.Commands.RemoveArea;

public class RemoveClientAreaCommandHandler : IRequestHandler<RemoveClientAreaCommand>
{
    private readonly IClientRepository repository;
    private readonly ICurrentUser currentUser;

    public RemoveClientAreaCommandHandler(IClientRepository repository, ICurrentUser currentUser)
    {
        this.repository = repository;
        this.currentUser = currentUser;
    }

    public async Task Handle(RemoveClientAreaCommand request, CancellationToken cancellationToken)
    {
        var client = await repository.GetWithAreas(Guid.Parse(currentUser.ClientId));
        if (client is null)
            throw new NotFoundException(nameof(Client), currentUser.ClientId);

        ClientArea area = client.Areas.FirstOrDefault(a => a.Id == request.Id);
        if (area is null)
            throw new NotFoundException(nameof(ClientArea), request.Id);

        client.RemoveArea(area);

        repository.Update(client);

        await repository.UnitOfWork.SaveChangesAsync(cancellationToken);

    }
}