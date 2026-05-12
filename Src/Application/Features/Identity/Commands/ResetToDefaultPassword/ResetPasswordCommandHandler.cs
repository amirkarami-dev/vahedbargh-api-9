using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Application.Common.Exceptions;
using Coreapi.Application.Common.Interfaces;
using MediatR;

namespace Coreapi.Application.Features.Identity.Commands.ResetToDefaultPassword
{
    public class ResetPasswordCommandHandler:IRequestHandler<ResetToDefaultPasswordCommand, string>
    {

        private readonly IUserManager _userManager;

        public ResetPasswordCommandHandler(IUserManager userManager)
        {
            _userManager = userManager;
        }
        public async Task<string> Handle(ResetToDefaultPasswordCommand request, CancellationToken cancellationToken)
        {

            var resultChange = await _userManager.ResetDefaultPassword(request.UserName);
            if (!resultChange) throw new NotAllowedException("رمز عبور قابل بازیابی نمی باشد");

            return "رمز جدید به موبایل شما ارسال گردید";
        }

        
    }
}
