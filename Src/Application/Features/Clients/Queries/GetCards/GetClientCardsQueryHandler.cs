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

namespace Coreapi.Application.Features.Clients.Queries.GetCards
{
    public class GetClientCardsQueryHandler : IRequestHandler<GetClientCardsQuery, IEnumerable<ClientCardDto>>
    {
        private readonly IClientRepository repository;
        private readonly ICurrentUser currentUser;
        private readonly IMapper mapper;

        public GetClientCardsQueryHandler(IClientRepository repository, ICurrentUser currentUser, IMapper mapper)
        {
            this.repository = repository;
            this.currentUser = currentUser;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ClientCardDto>> Handle(GetClientCardsQuery request, CancellationToken cancellationToken)
        {
            var client = await repository.GetWithCards(Guid.Parse(currentUser.ClientId));
            if (client is null)
                throw new NotFoundException(nameof(Client), currentUser.ClientId);

            return mapper.Map<IEnumerable<ClientCardDto>>(client.Cards);
        }
    }
}
