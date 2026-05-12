using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using MediatR;

namespace Coreapi.Application.Features.Clients.Commands.DeleteUser
{
    public class DeleteClientUserCommandHandler:IRequestHandler<DeleteClientUserCommand>
    {
        private readonly IUserManager usernaManager;
        private readonly ICurrentUser currentUser;

        public DeleteClientUserCommandHandler(IUserManager usernaManager, ICurrentUser currentUser)
        {
            this.usernaManager = usernaManager;
            this.currentUser = currentUser;
        }
        public async Task Handle(DeleteClientUserCommand request, CancellationToken cancellationToken)
        {
            var user = await usernaManager.GetUserAsync(request.Id);
            if (user is null)
            {
                throw new NotFoundException("User", request.Id);

            }

            if (!user.ClientId.Equals(currentUser.ClientId))
                throw new BadRequestException("Not Your User");
            await usernaManager.RemoveUserAsync(request.Id);
        }
    }
}
