using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Features.Clients.Queries.GetUserTrainings
{
    public class GetClientUserTrainingsQuery : IRequest<IEnumerable<UserTrainingDto>>
    {
        public Guid ClientId { get; set; }
        public string UserId { get; set; }
    }
}
