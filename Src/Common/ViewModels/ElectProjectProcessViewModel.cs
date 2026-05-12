using Coreapi.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Common.ViewModels
{
    public class ElectProjectProcessViewModel
    {
        public Guid Id { get; set; }

        public string EngName { get; set; }
        public string SolarDateDeliverEngineer { get;  set; }
        public DateTime? JulianDateDeliverEngineer { get; set; }

        public string SolarDateDeliverOffice { get;  set; }
        public DateTime? JulianDateDeliverOffice { get; set; }

        public bool Defect { get; set; }
        public string Description { get; set; }
        public string CellPhone { get; set; }
        public string InspectionStatusName { get; set; }

        public InspectionStatusEnum InspectionStatusEnum { get; set; }

        public ProjectLevelEnum ProjectLevelEnum { get; set; }
        public string ProjectLevelName { get; set; }


    }
}
