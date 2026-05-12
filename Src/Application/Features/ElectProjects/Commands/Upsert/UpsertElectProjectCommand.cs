using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Application.Common.Models.Request;
using Coreapi.Common.Enums;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using MediatR;

namespace Coreapi.Application.Features.ElectProjects.Commands.Upsert
{
    public class UpsertElectProjectCommand:IRequest<string>
    {
        public Guid? Id { get; init; }
        public int Area { get; set; }
        public int NumberOfFloor { get; set; }
        public string DesNumberOfFloor { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string LandlordName { get; set; }
        public string LandlordNaCode { get; set; }
        public string LandlordPhoneNumber { get; set; }
        public string CompanyName { get; set; }
        public string LicenseNumber { get; set; }
        public string Description { get; set; }
        public int IdSection { get; set; }
        public int IdCity { get; set; }
        public int IdProvince { get; set; }
        public float Lat { get; set; }
        public float Lng { get; set; }
        public RequesterTypeEnum RequesterTypeEnum { get; set; }
        public ProjectTypeRequestEnum ProjectTypeRequestEnum { get; set; }
        public ProjectCreatedTypeEnum ProjectCreatedTypeEnum { get; set; }
        public BuildingGroupTypeEnum BuildingGroupTypeEnum { get; set; }
        public BuildingGroupParameterTypeEnum BuildingGroupParameterTypeEnum { get; set; }
        public string SolarRegisterDate { get; set; }
        public bool IsEarthSystem { get; set; }
        public bool IsErtTest { get; set; }
        public bool IsBuildingInspection { get; set; }
        public bool IsTestAndDelivery { get; set; }
        public bool PanelNeed { get; set; }
        public string SupervisorName { get; set; }
        public string SupervisorPhoneNumber { get; set; }
        public ErtSystemTypeEnum ErtSystemTypeEnum { get; set; }
        public int ChildErtCount { get; set; }
        public int ChildInspectionCount { get; set; }
        public int FoundationElectrodeArea { get; set; }
        public bool IsNeedEb { get; set; }
        public long ElectRequestNumber { get; set; }
        public bool HasRelatedPermit { get; set; }
        public bool HasSupervision { get; set; }
        public int AreaAsBuilt { get; set; }

    }
}
