using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using MediatR;

namespace Coreapi.Application.Features.ElectProjectProcesses.Queries.GetProjectProcess
{
    public class GetProjectProcessQuery:IRequest<IEnumerable<GetProjectProcessDto>>
    {
        public Guid EpId { get; set; }
    }
}
