using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Enums;
using Coreapi.Common.Models;
using MediatR;

namespace Coreapi.Application.Features.ElectProjectProcesses.Queries.GetListProjectProcessEng
{
    public class GetListProjectProcessEngQuery:IRequest<PaggingList<ProjectProcessEngDto>>
    {
        public string SearchValue { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public long FileNumber { get; set; }
        public string SolarDateDeliverEngineer { get; set; }
        public int IdSection { get; set; } = 0;
        public InspectionStatusEnum InspectionStatusEnum { get; set; }
        public string LandlordName { get; set; }
        public string EngineerId { get; set; }
        
    }
}
