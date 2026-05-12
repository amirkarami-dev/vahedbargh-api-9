using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coreapi.Application.Features.Identity.Commands.LoginByMac
{
    public class LoginByMacCommand : IRequest<LoginDto>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string MacAddress { get; set; }
    }
}
