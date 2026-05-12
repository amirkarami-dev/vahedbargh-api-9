using Coreapi.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Common.RequestModel
{
    public class ElectProjectsFullFilterModel : BaseRequestModel
    {
        public ElectProjectsFullFilterModel(
            string searchValue, 
            string parentId, 
            long fileNumber,
            long electRequestNumber, 
            string solarRegisterDate, 
            int idSection,
            int idCity,
            bool filterProjectLevel, 
            ProjectLevelEnum projectLevelEnum,
            string landLordName,
            bool isStop,
            bool relatedPermitFilter,
            bool filterIsFilter,  
            bool isBuildingInspection, 
            bool isEarthSystem, 
            bool isTestAndDelivery,
            ElectProjectStatusEnum electProjectStatusEnum,
            bool isElectRequest,
            int page, 
            int pageSize
            )
        {
            SearchValue = searchValue;
            ParentId = parentId;
            FileNumber = fileNumber;
            ElectRequestNumber = electRequestNumber;
            SolarRegisterDate = solarRegisterDate;
            IdCity = idCity;
            IdSection = idSection;
            ProjectLevelEnum = projectLevelEnum;
            LandLordName = landLordName;
            IsStop = isStop;
            RelatedPermitFilter = relatedPermitFilter;
            FilterProjectLevel = filterProjectLevel;
            FilterIsFilter = filterIsFilter;
            IsBuildingInspection = isBuildingInspection;
            IsEarthSystem = isEarthSystem;
            IsTestAndDelivery = isTestAndDelivery;
            ElectProjectStatusEnum = electProjectStatusEnum;
            IsElectRequest = isElectRequest;
            Page = page;
            PageSize = pageSize;
        }
        public string SearchValue { get; set; }
        public long FileNumber { get; set; }
        public long ElectRequestNumber { get; set; }
        public string SolarRegisterDate { get; set; }
        public int IdSection { get; set; }
        public int IdCity { get; set; }
        public ProjectLevelEnum ProjectLevelEnum { get; set; }
        public string ExeName { get; set; }
        public string LandLordName { get; set; }
        public bool RelatedPermitFilter { get; set; }
        public bool IsStop { get; set; }
        public bool FilterProjectLevel { get; set; } = true;
        public bool FilterIsFilter { get; set; }
        public bool IsBuildingInspection { get; set; }
        public bool IsEarthSystem { get; set; }
        public bool IsTestAndDelivery { get; set; }
        public string ParentId { get; set; }
        public bool IsElectRequest { get; set; }
        public ElectProjectStatusEnum ElectProjectStatusEnum { get; set; }

    }
}
