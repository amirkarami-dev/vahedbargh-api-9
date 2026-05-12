using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coreapi.Application.Features.User.Commands.ChangeActivate
{
    public class ChangeUserActivateCommand : IRequest
    {
        public string UserId { get; set; }
    }
}
