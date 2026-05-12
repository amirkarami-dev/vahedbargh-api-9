using Coreapi.Common.Enums;
using System.Collections.Generic;
using System;

namespace Coreapi.Common.ViewModels;

public class ElectProjectViewModel
{
    public Guid Id { get; set; }
    public Guid? ParentId { get; set; }
    public ElectProjectParentModel ParentElectProject { get; set; }
    public long FileNumber { get; set; }
    public long ElectRequestNumber { get; set; }
    public string UserId { get; set; }
    public EngineerModel Engineer { get; set; }
    public PanelMakerModel? PanelMaker { get; set; }
    public InvoiceModel Invoice { get; set; }
    public int Area { get; set; }
    public int NumberOfFloor { get; set; }
    public string Address { get; set; }
    public string PostalCode { get; set; }
    public string LandlordName { get; set; }
    public string LandlordNaCode { get; set; }
    public string LandlordPhoneNumber { get; set; }
    public string SupervisorName { get; set; }
    public string SupervisorPhoneNumber { get; set; }
    public string CompanyName { get; set; }
    public string LicenseNumber { get; set; }
    public int IdSection { get; set; }
    public string EngCurrentName { get; set; }


    public bool PanelNeed { get; set; }
    public string PanelSerialNumber { get; set; }
    public bool PanelMakerSubmit { get; set; }
    public DateTime? JulianDatePanelRegister { get; set; }
    public string SolarDatePanelRegister { get; set; }
    public DateTime? JulianDatePanelSubmit { get; set; }
    public string SolarDatePanelSubmit { get; set; }


    public BuildingTypeEnum BuildingTypeEnum { get; set; }
    public ProjectLevelEnum ProjectLevelEnum { get; set; }
    public DateTime JulianRegisterDate { get; set; }
    public string SolarRegisterDate { get; set; }
    public string Description { get; set; }
    public DateTime? JulianDateDeliver { get; set; }
    public string SolarDateDeliver { get; set; }
    public DateTime? JulianDateSendToElectCo { get; set; }
    public string SolarDateSendToElectCo { get;  set; }
    public bool IsOk { get; set; }
    public float Lat { get; set; }
    public float Lng { get; set; }
    public bool Expired { get; set; }
    public bool IsUploadElectPlan { get; set; }
    public bool IsUploadRelatedPermit { get; set; }
    public bool IsDelete { get; set; }
    public int CountSendToElectCo { get; set; }
    public IEnumerable<ElectProjectProcessViewModel> ElectProjectProcessViewModel { get; set; }
    public string OwnershipType { get; set; }
    public BuildingGroupTypeEnum BuildingGroupTypeEnum { get; set; }
    public string BuildingGroupTypeName { get; set; }
    public BuildingGroupParameterTypeEnum BuildingGroupParameterTypeEnum { get; set; }
    public string BuildingGroupParameterTypeName { get; set; }
    public ProjectCreatedTypeEnum ProjectCreatedTypeEnum { get; set; }
    public string ProjectCreatedTypeName { get; set; }
    public ProjectTypeRequestEnum ProjectTypeRequestEnum { get; set; }
    public string ProjectTypeRequestName { get; set; }
    public string BuildingType { get; set; }
    public string ProjectLevel { get; set; }
    public string SolarDateDeliverEngineer { get; set; }
    public bool IsEarthSystem { get; set; }
    public bool IsErtTest { get; set; }
    public bool IsBuildingInspection { get; set; }
    public bool IsTestAndDelivery { get; set; }
    public bool IsStop { get; set; }
    public string StopDes { get; set; }
    public long ProjectBalance { get; set; }
    public string ProjectBalanceLinkForPay { get; set; }
    public long ProjectBalanceIn { get; set; }
    public long ProjectBalanceOut { get; set; }
    public ErtSystemTypeEnum ErtSystemTypeEnum { get; set; }
    public string ErtSystemTypeName { get; set; }

    public IEnumerable<CheckListForm> CheckListForms { get; set; }
    public IEnumerable<CheckListEdc> CheckListEdcs { get; set; }
    public IEnumerable<CommentEngForm> CommentEngForms { get; set; }
    public int CountChildren { get; set; }
    public Array Children { get; set; }
    public bool IsBigProject { get; set; }
    public long AmountPerArea { get; set; }
    public long foundationElectrodeArea { get; set; }
    public bool isNeedEb { get; set; }
    public ElectProjectStatusEnum ElectProjectStatusEnum { get; set; }
    public string ElectProjectStatusName { get; set; }
    public string DefectDes { get; set; }
    public bool IsDefectEng { get; set; }
    public bool SolvedDefectEng { get; set; }
    public DateTime? JulianDateDefectEng { get; set; }
    public string SolarDateDefectEng { get; set; }
    public DateTime? JulianDateSolvedDefectEng { get; set; }
    public string SolarDateSolvedDefectEng { get; set; }
    public string DefectEngDes { get; set; }
    public string DefectAdminDes { get; set; }
    public bool HasRelatedPermit { get; set; }
    public bool HasSupervision { get; set; }
    public int AreaAsBuilt { get; set; }
	public bool NeedElectNetwork { get; set; }


}

public class InvoiceModel   
{
    public Guid Id { get; init; }
    public long Amount { get; set; }
    public InvoiceStatusEnum InvoiceStatus { get; set; }
    public string InvoiceStatusName { get; set; }
    public InvoicePayTypeEnum InvoicePayType { get; set; }
    public string InvoicePayTypeName { get; set; }

}

public class EngProcessViewModel
{
    public Guid Id { get; set; }
    public string EngName { get; set; }
    public string SolarDateDeliverEngineer { get; set; }
    public DateTime? JulianDateDeliverEngineer { get; set; }
    public string SolarDateDeliverOffice { get; set; }
    public DateTime? julianDateDeliverOffice { get; set; }

    public bool Defect { get; set; }
    public string Description { get; set; }
    public InspectionStatusEnum InspectionStatusEnum { get; set; }
    public string InspectionStatusName { get; set; }
    public BuildingGroupParameterTypeEnum BuildingGroupParameterTypeEnum { get; set; }
    public string CellPhone { get; set; }
    public ProjectLevelEnum ProjectLevelEnum { get; set; }
    public string ProjectLevelName { get; set; }
}

public class PanelMakerModel
{
    public Guid Id { get; set; }
    public string CompanyName { get; set; }
    public string FullName { get; set; }
}
public class EngineerModel
{
    public string FullName { get; set; }
}

public class CheckListForm()
{
    public Guid Id { get; set; }
    public string SolarChecked { get; set; }
    public InspectionDesEnum InspectionDesEnum { get; set; }
    public string InspectionDesName { get; set; }
    public bool? IsComplete { get; set; }
    public string ResultDes { get; set; }
}
public class CheckListEdc()
{
    public Guid Id { get; set; }
    public string SolarChecked { get; set; }
    public CheckListEdcEnum CheckListEdcEnum { get; set; }
    public string CheckListEdcName { get; set; }
    public bool? IsComplete { get; set; }
    public string ResultDes { get; set; }
}

public class CommentEngForm()
{
    public Guid Id { get; set; }
    public BranchingTypeEnum BranchingTypeEnum { get; set; }
    public string BranchingTypeName { get; set; }
    public FazNumberEnum FazNumberEnum { get; set; }
    public string FazNumberName { get; set; }
    public int BranchingCount { get; set; }
    public float Ampere { get; set; }
    public float Power { get; set; }
    public float PowerSum { get; set; }
    public string Des { get; set; }
}

public class ElectProjectParentModel
{
    public Guid Id { get; set; }
    public long FileNumber { get; set; }
    public long ElectRequestNumber { get; set; }
    public string LandlordName { get; set; }
    public string Address { get; set; }
    public int Area { get; set; }
    public ProjectLevelEnum ProjectLevelEnum { get; set; }
    public string ProjectLevelName { get; set; }
    public string SolarRegisterDate { get; set; }
    public bool IsStop { get; set; }
    public ElectProjectStatusEnum ElectProjectStatusEnum { get; set; }
    public string ElectProjectStatusName { get; set; }
}