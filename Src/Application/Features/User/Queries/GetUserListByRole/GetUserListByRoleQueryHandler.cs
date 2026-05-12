using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Application.Common.Models;

namespace Coreapi.Application.Features.User.Queries.GetUserListByRole
{
    public class GetUserListByRoleQueryHandler : IRequestHandler<GetUserListByRoleQuery, List<UserData>>
    {
        private readonly IUserManager userManager;

        public GetUserListByRoleQueryHandler(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        public async Task<List<UserData>> Handle(GetUserListByRoleQuery request, CancellationToken cancellationToken)
        {
            if (!request.RoleName.Contains(","))
                return await userManager.GetUsers(request.RoleName);

            var roles = request.RoleName.Split(',');
            List<UserData> users = new ();

            foreach (var role in roles)
            {
                users.AddRange(await userManager.GetUsers(role.Trim()));
            }

            return users;
        }
    }
}
