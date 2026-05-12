using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Features.Clients.Queries.IsExist
{
    public class IsSubdomainExistQuery : IRequest<bool>
    {
        public string Subdomain { get; set; }
    }
}
