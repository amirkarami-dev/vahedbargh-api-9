using Coreapi.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Common.ViewModels
{
    public class ListElectProjectProcessViewModel
    {
        public string EnName { get; set; }
        public long Fee { get; set; }
        public string ProjectLevelName { get; set; }
        public short ProjectLevel { get; set; }
        public long FileNumber { get; set; }
        public long Id { get; set; }
        public int IdQuarterTariff { get; set; }
        public string IElectProject { get; set; }
        public string LandLordName { get; set; }
        public string LandlordNaCode { get; set; }
        public string LandlordPhoneNumber { get; set; }
        public string SolarDateDeliverEngineer { get; set; }
        public DateTime? JulianDateDeliverEngineer { get; set; }
        public string SolarDateDeliverOffice { get; set; }
        public DateTime? JulianDateDeliverOffice { get; set; }
        public string SolarRegisterDate { get; set; }
        public int? IdSection { get; set; }
        public short BuildingType { get; set; }
        public string BuildingTypeName { get; set; }
        public InspectionStatusEnum InspectionStatus { get; set; }
        public string InspectionStatusName { get; set; }
        public long IdExecutor { get; set; }
        public long IdEngineer { get; set; }
        public string ExecutorName { get; set; }
        public string ExecutorCellPhone { get; set; }
        public string EngineerCellPhone { get; set; }
        public string Description { get; set; }
        public bool Defect { get; set; }
        public bool PackageNeed { get; set; }
        public string PackageSerialNumber { get; set; }
        public string PackageCompanyName { get; set; }
        public string PackageInstallerName { get; set; }
        public string PackageInstallerPhoneNumber { get; set; }
        public bool PackageInstallerSubmit { get; set; }
        public BuildingGroupParameterTypeEnum BuildingGroupParameterType { get; set; }
        public int UnitNumber { get; set; }
        public int NumberOfFloor { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public int Area { get; set; }
        public string PipingTypeName { get; set; }
        public string Des1 { get; set; }
        public string Des2 { get; set; }
    }
}
