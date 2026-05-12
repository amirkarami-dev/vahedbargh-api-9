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

namespace Coreapi.Application.Features.Clients.Queries.GetAnalyzerClients
{
    public class GetAnalyzerClientsQueryHandler : IRequestHandler<GetAnalyzerClientsQuery, IEnumerable<AnalyzerClientDto>>
    {
        private readonly IClientRepository repository;
        private readonly ICurrentUser currentUser;
        private readonly IMapper mapper;

        public GetAnalyzerClientsQueryHandler(IClientRepository repository, ICurrentUser currentUser, IMapper mapper)
        {
            this.repository = repository;
            this.currentUser = currentUser;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<AnalyzerClientDto>> Handle(GetAnalyzerClientsQuery request, CancellationToken cancellationToken)
        {
            if (!currentUser.Role.Contains("SuperUser"))
                request.UserId = currentUser.UserId;

            var clients = await repository.GetAnalyzerClients(request.UserId);

            return mapper.Map<List<AnalyzerClientDto>>(clients);
        }
    }
}
