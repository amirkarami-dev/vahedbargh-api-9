using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using Coreapi.Domain.AggregatesModel.PanelMakerAgg;
using MediatR;

namespace Coreapi.Application.Features.PanelMakers.Queries.GetClientPanelMakers
{
    public class GetClientPanelMakerQueryHandler : IRequestHandler<GetClientPanelMakerQuery, IEnumerable<PanelMakerDto>>
    {
        private readonly IPanelMakerRepository panelMakerRepository;
        private readonly ICurrentUser currentUser;
        private readonly IMapper mapper;
        private readonly IClientRepository clientRepository;

        public GetClientPanelMakerQueryHandler(IPanelMakerRepository panelMakerRepository, ICurrentUser currentUser, IMapper mapper, IClientRepository clientRepository)
        {
            this.panelMakerRepository = panelMakerRepository;
            this.currentUser = currentUser;
            this.mapper = mapper;
            this.clientRepository = clientRepository;
        }
        public async Task<IEnumerable<PanelMakerDto>> Handle(GetClientPanelMakerQuery request, CancellationToken cancellationToken)
        {
            var client = await clientRepository.GetWithSetting(Guid.Parse(currentUser.ClientId));
            if (client is null)
                throw new NotFoundException(nameof(Client), currentUser.ClientId);

            var packageInstallers = await panelMakerRepository.GetClientPanelMakers(client.Id);
             return mapper.Map<IEnumerable<PanelMakerDto>>(packageInstallers);
        }
    }
}

