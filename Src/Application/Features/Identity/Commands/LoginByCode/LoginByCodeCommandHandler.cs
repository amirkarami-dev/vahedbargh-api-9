using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Interfaces;
using Coreapi.Application.Common.Models;
using MediatR;

namespace Coreapi.Application.Features.Identity.Commands.LoginByCode
{
    public class LoginByCodeCommandHandler:IRequestHandler<LoginByCodeCommand, LoginOutput>
    {
        private readonly ISignInManager signInManager;

        public LoginByCodeCommandHandler(ISignInManager signInManager)
        {
            this.signInManager = signInManager;
        }
        public async Task<LoginOutput> Handle(LoginByCodeCommand request, CancellationToken cancellationToken)
        {
            var output = await signInManager.Login(request.Code, request.UserName);
            return output;
        }
    }
}
