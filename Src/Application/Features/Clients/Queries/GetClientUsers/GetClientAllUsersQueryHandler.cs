using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Interfaces;

namespace Coreapi.Application.Features.Clients.Queries.GetClientUsers
{
    public class GetClientAllUsersQueryHandler : IRequestHandler<GetClientAllUsersQuery, IEnumerable<ClientUserDto>>
    {
        private readonly IUserManager userManager;
        private readonly ICurrentUser currentUser;
        private readonly IMapper mapper;

        public GetClientAllUsersQueryHandler(IUserManager userManager, ICurrentUser currentUser, IMapper mapper)
        {
            this.userManager = userManager;
            this.currentUser = currentUser;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ClientUserDto>> Handle(GetClientAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await userManager.GetClientUsers(string.IsNullOrEmpty(currentUser.ClientId) ? request.ClientId.ToString() : currentUser.ClientId);

            return mapper.Map<IEnumerable<ClientUserDto>>(users);
        }
    }
}
