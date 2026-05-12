using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coreapi.Application.Features.Identity.Commands.ChangePassword
{
    public class ChangePasswordCommand : IRequest
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
