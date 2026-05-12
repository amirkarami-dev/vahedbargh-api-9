using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Enums;
using Coreapi.Common.Models;
using Coreapi.Common.ViewModels;
using MediatR;

namespace Coreapi.Application.Features.ElectProjects.Queries.GetProjectsFullFilter
{
    public class GetElectProjectsFullFilterQuery : IRequest<PaggingList<ElectProjectViewModel>>
    {
        public string SearchValue { get; set; }
        public string ParentId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public long FileNumber { get; set; }
        public long ElectRequestNumber { get; set; }
        public string SolarRegisterDate { get; set; }
        public int IdSection { get; set; } = 0;
        public ProjectLevelEnum ProjectLevelEnum { get; set; }
        public string EngName { get; set; }
        public string LandLordName { get; set; }
        public bool RelatedPermitFilter { get; set; }
        public bool FilterProjectLevel { get; set; } = true;
        public bool IsStop { get; set; } = false;
        public bool FilterIsFilter { get; set; } = false;
        public bool IsBuildingInspection { get; set; } = false;
        public bool IsEarthSystem { get; set; } = false;
        public bool IsTestAndDelivery { get; set; } = false;
        public ElectProjectStatusEnum ElectProjectStatusEnum { get; set; } = ElectProjectStatusEnum.None;


    }
}
