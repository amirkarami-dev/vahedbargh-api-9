using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Interfaces;

namespace Coreapi.Application.Features.User.Queries.GetUserRoles
{
    public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, List<string>>
    {
        private readonly IUserManager userManager;

        public GetUserRolesQueryHandler(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        public async Task<List<string>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await userManager.GetUserRolesAsync(request.UserId);

            return roles.ToList();
        }
    }
}
