using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Application.Common.Models;
using MediatR;

namespace Coreapi.Application.Features.Identity.Commands.LoginByCode
{
    public class LoginByCodeCommand:IRequest<LoginOutput>
    {
        public string Code { get; set; }
        public string UserName { get; set; }
    }
}
