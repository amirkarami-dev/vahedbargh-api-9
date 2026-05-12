using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Features.Clients.Commands.RemoveArea;

public class RemoveClientAreaCommand : IRequest
{
    public Guid Id { get; set; }
}
