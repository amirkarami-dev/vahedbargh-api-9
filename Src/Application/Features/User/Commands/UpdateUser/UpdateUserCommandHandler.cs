using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Interfaces;

namespace Coreapi.Application.Features.User.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly IUserManager userManager;

        public UpdateUserCommandHandler(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            await userManager.UpdateUser(request.UserId, request.FirstName, request.LastName);

        }
    }
}
