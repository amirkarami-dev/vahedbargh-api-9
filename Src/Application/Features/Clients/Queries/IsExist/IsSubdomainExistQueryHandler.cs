using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Domain.AggregatesModel.ClientAggregate;

namespace Coreapi.Application.Features.Clients.Queries.IsExist
{
    public class IsSubdomainExistQueryHandler : IRequestHandler<IsSubdomainExistQuery, bool>
    {
        private readonly IClientRepository repository;

        public IsSubdomainExistQueryHandler(IClientRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(IsSubdomainExistQuery request, CancellationToken cancellationToken)
        {
            return await repository.SubdomainExist(request.Subdomain.ToLower().Trim());
        }
    }
}
