using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Features.Clients.Commands.AssignAreaUsers;

public class AssignClientAreaUsersCommand : IRequest
{
    public Guid AreaId { get; set; }
    public IEnumerable<string> Users { get; set; } = Enumerable.Empty<string>();
}
