using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Features.Clients.Commands.UpsertTraining
{
    public class UpsertClientUserTrainingCommand : IRequest
    {
        public Guid? TrainingId { get; set; }
        public Guid ClientId { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
    }
}
