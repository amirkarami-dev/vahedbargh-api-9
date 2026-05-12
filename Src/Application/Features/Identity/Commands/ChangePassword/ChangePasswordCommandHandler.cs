using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Interfaces;

namespace Coreapi.Application.Features.Identity.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand>
    {
        private readonly IUserManager userManager;
        private readonly ICurrentUser currentUser;

        public ChangePasswordCommandHandler(IUserManager userManager,ICurrentUser currentUser)
        {
            this.userManager = userManager;
            this.currentUser = currentUser;
        }

        public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            await userManager.ChangePassword(currentUser.UserId, request.OldPassword, request.NewPassword);

        }
    }
}
