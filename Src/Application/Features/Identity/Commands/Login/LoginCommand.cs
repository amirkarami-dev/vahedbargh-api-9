using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Coreapi.Common.Enums;

namespace Coreapi.Application.Features.Identity.Commands.Login
{
    public class LoginCommand : IRequest<LoginDto>
    {
        public bool OneMinute { get; set; } = false;
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DeviceToken { get; set; } = string.Empty;
        public PlatformTypeEnum PlatformType { get; set; } = PlatformTypeEnum.Android;
    }
}
