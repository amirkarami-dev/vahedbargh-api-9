using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coreapi.Application.Features.User.Commands.ResetPassword
{
    public class ResetPasswordCommand : IRequest
    {
        public string UserId { get; set; }
        public string Password { get; set; }
    }
}
