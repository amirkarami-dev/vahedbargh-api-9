using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Features.Clients.Commands.UpdateUserRole
{
    public class UpdateClientUserRoleCommand : IRequest
    {
        public string UserId { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
