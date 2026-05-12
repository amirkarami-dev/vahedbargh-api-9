using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coreapi.Application.Features.User.Commands.AddUser
{
    public class AddUserCommand : IRequest<string>
    {
        public long NaCode { get; set; }
        public string UserName { get; set; }
        public string TypeUser { get; set; }
        public string MobileNumber { get; set; }
    }
}
