using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Features.Clients.Queries.GetMicrosoftUsers
{
    public class GetMicrosoftUsersQuery : IRequest<IEnumerable<MicrosoftUserDto>>
    {
        public string Token { get; set; }
    }
}
