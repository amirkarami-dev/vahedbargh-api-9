using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;

namespace Coreapi.Application.Features.User.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
    {
        private readonly IUserManager userManager;
        private readonly ICurrentUser currentUser;

        public ResetPasswordCommandHandler(IUserManager userManager, ICurrentUser currentUser)
        {
            this.userManager = userManager;
            this.currentUser = currentUser;
        }

        public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            if (!currentUser.Role.Contains("Administrator") && !await userManager.IsInClient(currentUser.UserId, request.UserId))
                throw new BadRequestException("Not Your User");

            await userManager.ResetPassword(request.UserId, request.Password);

        }
    }
}
