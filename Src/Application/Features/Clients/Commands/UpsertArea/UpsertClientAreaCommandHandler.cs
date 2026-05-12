using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Common.Enums;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.ValueObjects;

namespace Coreapi.Application.Features.Clients.Commands.UpsertArea;

public class UpsertClientAreaCommandHandler : IRequestHandler<UpsertClientAreaCommand>
{
    private readonly IClientRepository repository;
    private readonly ICurrentUser currentUser;

    public UpsertClientAreaCommandHandler(IClientRepository repository, ICurrentUser currentUser)
    {
        this.repository = repository;
        this.currentUser = currentUser;
    }

    public async Task Handle(UpsertClientAreaCommand request, CancellationToken cancellationToken)
    {
        var client = await repository.GetWithAreas(Guid.Parse(currentUser.ClientId));
        if (client is null)
            throw new NotFoundException(nameof(Client), currentUser.ClientId);

        ClientArea area = null;
        if (request.Id.HasValue)
            area = client.Areas.FirstOrDefault(a => a.Id == request.Id.Value);

        if (area is null)
        {
            var status = request.Type == SegmentTypeEnum.Polygon ? client.AddArea(request.Name, request.Description, request.Area.Select(a => new Point(a.Latitude, a.Longitude)))
                : client.AddArea(request.Name, request.Description, request.Radius, request.Area.Select(a => new Point(a.Latitude, a.Longitude)).FirstOrDefault());

            if (status.HasErrors)
                throw new BadRequestException(status.GetError());
        }
        else if (area.Type == SegmentTypeEnum.Polygon)
            area.Update(request.Name, request.Description, request.Area.Select(a => new Point(a.Latitude, a.Longitude)));
        else
            area.Update(request.Name, request.Description, request.Radius, request.Area.Select(a => new Point(a.Latitude, a.Longitude)).FirstOrDefault());

        repository.Update(client);

        await repository.UnitOfWork.SaveChangesAsync(cancellationToken);

    }
}
