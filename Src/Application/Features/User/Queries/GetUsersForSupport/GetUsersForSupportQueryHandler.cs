using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Interfaces;

namespace Coreapi.Application.Features.User.Queries.GetUsersForSupport
{
    public class GetUsersForSupportQueryHandler(IUserManager userManager, ICurrentUser currentUser)
        : IRequestHandler<GetUsersForSupportQuery, List<UserDataDto>>
    {
        public async Task<List<UserDataDto>> Handle(GetUsersForSupportQuery request, CancellationToken cancellationToken)
        {
        List<UserDataDto> result;

       // Check if current user is Administrator
            if (currentUser.Role.Contains("Administrator"))
            {
                // Return all users except herself
                var allUsers = await userManager.GetClientUsersByAdministrator(currentUser.UserId);
                
                result = allUsers
                    .Where(u => u.Id != currentUser.UserId)
                    .Where(u=>u.Role =="Engineer")
                    .Select(u => new UserDataDto
                    {
                        Id = u.Id,
                        FirstName = u.FirstName,
                        LastName = u.LastName
                    })
                    .ToList();
            }
            else
            {
                // Return all users that contain Administrator role
                var adminUsers = await userManager.GetClientAdmins(currentUser.ClientId);
            
                result = adminUsers.Where(w=>w.IsAdminSupport)
                    .Select(u => new UserDataDto
                    {
                        Id = u.Id,
                        FirstName = u.FirstName,
                        LastName = u.LastName
                    })

                    .ToList();
            }

       return result;
        }
    }
}
