using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coreapi.Application.Features.Identity.Commands.LoginByMicrosoft
{
    public class LoginByMicrosoftCommand : IRequest<LoginDto>
    {
        public string TokenId { get; set; }
    }
}
