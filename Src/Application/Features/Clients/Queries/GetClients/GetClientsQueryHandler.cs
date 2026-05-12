using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Common.Models;
using Coreapi.Domain.AggregatesModel.ClientAggregate;

namespace Coreapi.Application.Features.Clients.Queries.GetClients
{
    public class GetClientsQueryHandler : IRequestHandler<GetClientsQuery, PaggingList<ClientDto>>
    {
        private readonly IClientRepository repository;
        private readonly IUserManager userManager;
        private readonly IMapper mapper;

        public GetClientsQueryHandler(IClientRepository repository, IUserManager userManager, IMapper mapper)
        {
            this.repository = repository;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        public async Task<PaggingList<ClientDto>> Handle(GetClientsQuery request, CancellationToken cancellationToken)
        {
            var clients = await repository.GetAll(request.Name, request.PageNumber - 1, request.RowCount);

            var data = new List<ClientDto>();

            foreach (var client in clients)
            {
                var clientDto = mapper.Map<ClientDto>(client);

                clientDto.UsersCount = await userManager.GetClientUsersCount(client.Id.ToString());

                data.Add(clientDto);
            }

            return new PaggingList<ClientDto>
            {
                Data = data,
                TotalItems = await repository.Count(request.Name)
            };
        }
    }
}
