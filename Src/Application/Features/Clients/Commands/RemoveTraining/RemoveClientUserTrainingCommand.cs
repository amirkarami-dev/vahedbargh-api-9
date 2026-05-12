using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Features.Clients.Commands.RemoveTraining
{
    public class RemoveClientUserTrainingCommand : IRequest
    {
        public Guid ClientId { get; set; }
        public Guid TrainingId { get; set; }
    }
}
