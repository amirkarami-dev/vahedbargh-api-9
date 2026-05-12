using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Coreapi.Application.Features.User.Queries.GetUserFile
{
    public class GetUserFileQuery:IRequest<byte[]>
    {
        public string Path { get; set; }
    }
}
