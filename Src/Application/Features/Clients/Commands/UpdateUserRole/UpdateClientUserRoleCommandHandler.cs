using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;

namespace Coreapi.Application.Features.Clients.Commands.UpdateUserRole
{
    public class UpdateClientUserRoleCommandHandler : IRequestHandler<UpdateClientUserRoleCommand>
    {
        private readonly IUserManager userManager;
        private readonly ICurrentUser currentUser;

        public UpdateClientUserRoleCommandHandler(IUserManager userManager, ICurrentUser currentUser)
        {
            this.userManager = userManager;
            this.currentUser = currentUser;
        }

        public async Task Handle(UpdateClientUserRoleCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.GetUserAsync(request.UserId);
            if (user is null)
                throw new NotFoundException("User", request.UserId);

            if (!user.ClientId.Equals(currentUser.ClientId))
                throw new BadRequestException("Not Your User");
                
            await userManager.RemoveFromRoleAsync(request.UserId);

            await userManager.AddToRolesAsync(request.UserId, request.Roles);

        }
    }
}
