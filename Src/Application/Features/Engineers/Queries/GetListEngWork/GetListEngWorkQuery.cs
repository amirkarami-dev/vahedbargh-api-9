using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Enums;
using Coreapi.Common.ViewModels;
using MediatR;

namespace Coreapi.Application.Features.Engineers.Queries.GetListEngWork
{
    public class GetListEngWorkQuery:IRequest<IEnumerable<ClientEngWorkViewModel>>
    {
        public string EngId { get; set; }
        public string QtId { get; set; }
    }
}
