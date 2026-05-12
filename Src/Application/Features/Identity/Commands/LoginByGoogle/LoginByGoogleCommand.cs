using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coreapi.Application.Features.Identity.Commands.LoginByGoogle
{
    public class LoginByGoogleCommand : IRequest<LoginDto>
    {
        public string TokenId { get; set; }
    }
}
