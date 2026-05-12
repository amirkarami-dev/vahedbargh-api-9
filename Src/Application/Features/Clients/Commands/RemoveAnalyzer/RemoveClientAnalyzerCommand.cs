using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Features.Clients.Commands.RemoveAnalyzer
{
    public class RemoveClientAnalyzerCommand : IRequest
    {
        public string UserId { get; set; }
        public Guid ClientId { get; set; }
    }
}
