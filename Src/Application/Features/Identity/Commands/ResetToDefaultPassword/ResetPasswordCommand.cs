using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Coreapi.Application.Features.Identity.Commands.ResetToDefaultPassword
{
    public class ResetToDefaultPasswordCommand : IRequest<string>  
    {
        public string UserName { get; set; }
    }
}
