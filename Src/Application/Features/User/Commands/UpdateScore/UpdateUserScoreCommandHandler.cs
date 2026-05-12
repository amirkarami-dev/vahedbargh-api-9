using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Interfaces;

namespace Coreapi.Application.Features.User.Commands.UpdateScore
{
    public class UpdateUserScoreCommandHandler : IRequestHandler<UpdateUserScoreCommand>
    {
        private readonly IUserManager userManager;

        public UpdateUserScoreCommandHandler(IUserManager userManager)
        {
            this.userManager = userManager;
        }

        public async Task Handle(UpdateUserScoreCommand request, CancellationToken cancellationToken)
        {
            await userManager.UpdateScore(request.UserId, request.Score);

        }
    }
}
