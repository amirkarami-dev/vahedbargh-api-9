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

namespace Coreapi.Application.Features.Clients.Queries.GetMicrosoftUsers
{
    public class GetMicrosoftUsersQueryHandler : IRequestHandler<GetMicrosoftUsersQuery, IEnumerable<MicrosoftUserDto>>
    {
        private readonly IClientRepository repository;
        private readonly ICurrentUser currentUser;
        private readonly IMicrosoftService microsoftService;

        public GetMicrosoftUsersQueryHandler(IClientRepository repository, ICurrentUser currentUser, IMicrosoftService microsoftService)
        {
            this.repository = repository;
            this.currentUser = currentUser;
            this.microsoftService = microsoftService;
        }

        public async Task<IEnumerable<MicrosoftUserDto>> Handle(GetMicrosoftUsersQuery request, CancellationToken cancellationToken)
        {
            var client = await repository.GetWithPackages(Guid.Parse(currentUser.ClientId));
            if (client is null)
                throw new NotFoundException(nameof(Client), currentUser.ClientId);

            var result = await microsoftService.GetUsers(request.Token);

            if (result is null)
                throw new BadRequestException("UnsuccessfulImport");

            return result.Select(r => new MicrosoftUserDto
            {
                Email = r.Email,
                FirstName = r.Firstname,
                LastName = r.Lastname
            });
        }
    }
}
