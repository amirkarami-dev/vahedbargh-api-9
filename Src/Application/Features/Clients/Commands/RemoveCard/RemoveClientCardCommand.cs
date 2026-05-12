using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Features.Clients.Commands.RemoveCard
{
    public class RemoveClientCardCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
