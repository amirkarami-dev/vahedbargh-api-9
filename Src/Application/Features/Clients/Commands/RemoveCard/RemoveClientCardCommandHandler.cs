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

namespace Coreapi.Application.Features.Clients.Commands.RemoveCard
{
    public class RemoveClientCardCommandHandler : IRequestHandler<RemoveClientCardCommand>
    {
        private readonly IClientRepository repository;
        private readonly ICurrentUser currentUser;

        public RemoveClientCardCommandHandler(IClientRepository repository, ICurrentUser currentUser)
        {
            this.repository = repository;
            this.currentUser = currentUser;
        }

        public async Task Handle(RemoveClientCardCommand request, CancellationToken cancellationToken)
        {
            var client = await repository.GetWithCards(Guid.Parse(currentUser.ClientId));
            if (client is null)
                throw new NotFoundException(nameof(Client), currentUser.ClientId);

            var status = client.RemoveCard(request.Id);
            if (status.HasErrors)
                throw new BadRequestException(status.GetError());

            await repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        }
    }
}
