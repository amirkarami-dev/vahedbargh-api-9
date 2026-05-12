using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;

namespace Coreapi.Application.Features.User.Commands.ChangeActivate
{
    public class ChangeUserActivateCommandHandler : IRequestHandler<ChangeUserActivateCommand>
    {
        private readonly IUserManager userManager;
        private readonly ICurrentUser currentUser;

        public ChangeUserActivateCommandHandler(IUserManager userManager, ICurrentUser currentUser)
        {
            this.userManager = userManager;
            this.currentUser = currentUser;
        }

        public async Task Handle(ChangeUserActivateCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.Role.Contains("SuperUser") && !await userManager.IsInClient(currentUser.UserId, request.UserId))
                throw new BadRequestException("Not Your User");

            await userManager.UpdateActivate(request.UserId);

        }
    }
}
