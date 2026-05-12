using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Application.Common.Models;

namespace Coreapi.Application.Features.User.Queries.GetLeaderboardByRole
{
    public class GetLeaderboardByRoleQueryHandler : IRequestHandler<GetLeaderboardByRoleQuery, List<UserData>>
    {
        private readonly IUserManager userManager;

        public GetLeaderboardByRoleQueryHandler(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        public async Task<List<UserData>> Handle(GetLeaderboardByRoleQuery request, CancellationToken cancellationToken)
        {
            
            return await userManager.GetLeaderboard(request.RoleName);
        }
    }
}
