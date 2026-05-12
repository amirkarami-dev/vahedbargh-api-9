using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Coreapi.Application.Features.User.Queries.GetClientUserFiles
{
    public class GetClientUserFilesQuery:IRequest<IEnumerable<GetUserFilesDto>>
    {
        public string UserId { get; set; }
    }
}
