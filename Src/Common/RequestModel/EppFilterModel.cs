using Coreapi.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Common.RequestModel
{
    public class EppFilterModel: BaseRequestModel
    {
        public EppFilterModel(string searchValue, long fileNumber, string solarDateDeliverEngineer,string landlordName, int idSection, InspectionStatusEnum inspectionStatusEnum, int page, int pageSize, string engineerId)
        {
            SearchValue = searchValue;
            FileNumber = fileNumber;
            SolarDateDeliverEngineer = solarDateDeliverEngineer;
            IdSection = idSection;
            LandlordName = landlordName;
            InspectionStatusEnum = inspectionStatusEnum;
            Page = page;
            PageSize = pageSize;
            EngineerId = engineerId;
        }
        public string SearchValue { get; set; }
        public long FileNumber { get; set; }
        public string SolarDateDeliverEngineer { get; set; }
        public int IdSection { get; set; }
        public InspectionStatusEnum InspectionStatusEnum { get; set; }
        public string LandlordName { get; set; }
        public string EngineerId { get; set; }
        
    }
}
