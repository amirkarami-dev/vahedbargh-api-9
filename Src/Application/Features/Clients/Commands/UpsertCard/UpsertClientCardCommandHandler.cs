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

namespace Coreapi.Application.Features.Clients.Commands.UpsertCard
{
    public class UpsertClientCardCommandHandler : IRequestHandler<UpsertClientCardCommand, Guid>
    {
        private readonly IClientRepository repository;
        private readonly ICurrentUser currentUser;

        public UpsertClientCardCommandHandler(IClientRepository repository, ICurrentUser currentUser)
        {
            this.repository = repository;
            this.currentUser = currentUser;
        }

        public async Task<Guid> Handle(UpsertClientCardCommand request, CancellationToken cancellationToken)
        {
            var client = await repository.GetWithCards(Guid.Parse(currentUser.ClientId));
            if (client is null)
                throw new NotFoundException(nameof(Client), currentUser.ClientId);

            var card = client.Cards.FirstOrDefault(c => c.Id == request.Id);
            if(card is null)
            {
                if (await repository.IsCardExist(request.CardNumber, request.Type))
                    throw new BadRequestException("Card Number Exist");

                client.AddCard(request.NameOnCard, request.CardNumber, request.Type, request.CCV, request.ExpireDate);

                card = client.Cards.First(c => c.CardNumber.Equals(request.CardNumber) && c.Type == request.Type);
            }
            else
            {
                if (!card.CardNumber.Equals(request.CardNumber) && await repository.IsCardExist(request.CardNumber, card.Type))
                    throw new BadRequestException("Card Number Exist");

                card.Update(request.NameOnCard, request.CardNumber, request.CCV, request.ExpireDate);
            }

            await repository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return card.Id;
        }
    }
}
