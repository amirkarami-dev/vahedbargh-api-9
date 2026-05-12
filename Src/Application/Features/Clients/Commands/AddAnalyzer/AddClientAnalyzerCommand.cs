using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Features.Clients.Commands.AddAnalyzer
{
    public class AddClientAnalyzerCommand : IRequest
    {
        public string UserId { get; set; }
        public IEnumerable<Guid> Clients { get; set; } = new List<Guid>();
    }
}
