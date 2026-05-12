using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coreapi.Application.Features.Identity.Commands.RefreshToken
{
    public class RefreshTokenCommand : IRequest<RefreshTokenDto>
    {
        public string UserId { get; set; }
        public string RefreshToken { get; set; }
        public bool OneMinute { get; set; } = false;
    }
}
