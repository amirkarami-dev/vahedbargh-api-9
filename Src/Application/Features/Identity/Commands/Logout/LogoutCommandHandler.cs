using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Interfaces;
using MediatR;

namespace Coreapi.Application.Features.Identity.Commands.Logout
{
    public class LogoutCommandHandler:IRequestHandler<LogoutCommand>
    {
        private readonly ISignInManager sinInManager;
        private readonly ICurrentUser currentUser;

        public LogoutCommandHandler(ISignInManager sinInManager, ICurrentUser currentUser)
        {
            this.sinInManager = sinInManager;
            this.currentUser = currentUser;
        }
        public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            _ = sinInManager.RemoveSessionToken(currentUser.UserId);
        }
    }
}
