using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Coreapi.Application.Features.Clients.Commands.DeleteUser
{
    public class DeleteClientUserCommand:IRequest     
    {
        public string Id { get; set; }
    }
}
