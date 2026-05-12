using Coreapi.Common.Enums;
using Coreapi.Domain.AggregatesModel.ClientAggregate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Coreapi.Common.Utility;
using Coreapi.Domain.AggregatesModel.BuildingTariffAgg;
using Coreapi.Domain.AggregatesModel.ErtTariffAgg;
using Coreapi.Domain.AggregatesModel.GeoAgg;
using Coreapi.Domain.AggregatesModel.PanelMakerAgg;

namespace Coreapi.Domain.AggregatesModel.ElectProjectAgg;

public class ElectProject
{
    private ElectProject(){}

    public ElectProject( 
        long fileNumber, string userId, 
        Client client, int area, int numberOfFloor, 
        string desNumberOfFloor, string address, 
        string postalCode, string landlordName, 
        string landlordNaCode, string landlordPhoneNumber, 
        string companyName, string licenseNumber, 
        string description, int idSection, int idCity, 
        int idProvince, float lat, float lng, 
        ProjectCreatedTypeEnum projectCreatedTypeEnum, 
        ProjectTypeRequestEnum projectTypeRequestEnum, 
        BuildingTariff buildingTariff,ErtTariff ertTariff, 
        ProjectLevelEnum projectLevelEnum, DateTime julianRegisterDate,
        string solarRegisterDate, bool isEarthSystem, bool isErtTest, 
        bool isBuildingInspection, bool isTestAndDelivery, 
        bool panelNeed, int foundationElectrodeArea,bool isNeedEb,
        long electRequestNumber,
        bool hasRelatedPermit,
        bool hasSupervision,
        int areaAsBuilt
        )
    {
        Id = Guid.NewGuid();
        FileNumber = fileNumber;
        UserId = userId;
        Client = client;
        Area = area;
        NumberOfFloor = numberOfFloor;
        DesNumberOfFloor = desNumberOfFloor;
        Address = address;
        PostalCode = postalCode;
        LandlordName = landlordName;
        LandlordNaCode = landlordNaCode;
        LandlordPhoneNumber = landlordPhoneNumber;
        CompanyName = companyName;
        LicenseNumber = licenseNumber;
        Description = description;
        IdSection = idSection;
        IdCity = idCity;
        IdProvince = idProvince;
        Lat = lat;
        Lng = lng;
        ProjectCreatedTypeEnum = projectCreatedTypeEnum;
        ProjectTypeRequestEnum = projectTypeRequestEnum;
        ProjectLevelEnum = projectLevelEnum;
        JulianRegisterDate = julianRegisterDate;
        SolarRegisterDate = solarRegisterDate;
        IsOk = false;
        IsEarthSystem = isEarthSystem;
        IsErtTest = isErtTest;
        IsBuildingInspection = isBuildingInspection;
        IsTestAndDelivery = isTestAndDelivery;
        IsStop = false;
        IsDelete = false;
        Expired = false;
        BuildingTariff = buildingTariff;
        ErtTariff = ertTariff;
        PanelMakerSubmit = false;
        PanelNeed = panelNeed;
        NeedElectNetwork = false;
        ParentProject = null;
        FoundationElectrodeArea = foundationElectrodeArea;
        IsNeedEb = isNeedEb;
        ElectProjectStatusEnum = ElectProjectStatusEnum.None;
        ElectRequestNumber = electRequestNumber;
        IsDefectEng = false;
        SolvedDefectEng = false;
        HasRelatedPermit = hasRelatedPermit;
        HasSupervision = hasSupervision;
        AreaAsBuilt = areaAsBuilt;
    }

    public Guid Id { get; init; }
    
    public  ElectProject ParentProject { get; set; }
    public  ICollection<ElectProject> ChildProjects { get; set; }

    public long FileNumber { get; private set; }
    public long ElectRequestNumber { get; set; }
    public string UserId { get; private set; }

    public Client Client { get; private set; }
    public PanelMaker PanelMaker { get; set; }
    public int Area { get; private set; }
    public int NumberOfFloor { get; private set; }
    public string DesNumberOfFloor { get; private set; }
    public string Address { get; private set; }
    public string PostalCode { get; private set; }
    public string LandlordName { get; private set; }
    public string LandlordNaCode { get; private set; }
    public string LandlordPhoneNumber { get; private set; }
    public string CompanyName { get; private set; }
    public string LicenseNumber { get; private set; }
    public string Description { get; private set; }
    public int IdSection { get; private set; }
    public int IdCity { get; set; }
    public int IdProvince { get; set; }
    public float Lat { get; private set; }
    public float Lng { get; private set; }

    public bool PanelNeed { get; set; }
    public string PanelSerialNumber { get; private set; }
    public bool PanelMakerSubmit { get; private set; }
    public DateTime? JulianDatePanelSubmit { get; private set; }
    public string SolarDatePanelSubmit { get; private set; }
    public DateTime? JulianDatePanelRegister { get; private set; }
    public string SolarDatePanelRegister { get; private set; }

    public RequesterTypeEnum RequesterTypeEnum { get; set; }
    public ProjectTypeRequestEnum ProjectTypeRequestEnum { get; private set; }
    public ProjectCreatedTypeEnum ProjectCreatedTypeEnum { get; private set; }
    public BuildingTariff BuildingTariff { get; set; }
    public ErtTariff ErtTariff { get; set; }
    public ProjectLevelEnum ProjectLevelEnum { get; private set; }
    public DateTime JulianRegisterDate { get; private set; }
    public string SolarRegisterDate { get; private set; }
    public DateTime? JulianDateDeliver { get; private set; }
    public string SolarDateDeliver { get; private set; }
    public DateTime? JulianDateSendToElectCo { get; private set; }
    public string SolarDateSendToElectCo { get; private set; }
    public int CountSendToElectCo { get; private set; }
    public bool IsEarthSystem { get; set; }
    public bool IsErtTest { get; set; }
    public bool IsBuildingInspection { get; set; }
    public bool IsTestAndDelivery { get; set; }
    public bool NeedElectNetwork { get; set; }
    public bool IsOk { get; private set; }
    public bool IsStop { get; private set; }
    public string StopDes { get; private set; }
    public bool IsDelete { get; private set; }
    public bool Expired { get; set; }
    public string SupervisorName { get; set; }
    public string SupervisorPhoneNumber { get; set; }
    public bool IsBigProject { get; set; }
    // در اینجا برای پروزه های بزرگ کل مبلغ محاسبه شده در این فیلد قرار میگیرد
    public long AmountPerArea { get; set; }
    public int FoundationElectrodeArea { get; set; }
    // Equipotential Bonding نیاز به همبندی
    public bool IsNeedEb { get; set; }
    public ElectProjectStatusEnum ElectProjectStatusEnum { get; set; }

    // توضیحات نقص شرکت توزیع
    public string DefectDes { get; set; }

    // نقص در مراحل تخصص کارشناسی
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

    public Section Section { get; set; }

    public BuildingTypeEnum BuildingTypeEnum { get; set; }
    public void Update(string licenseNumber,
        string landlordName, 
        string companyName, 
        string landlordNaCode, 
        string landlordPhoneNumber, 
        string postalCode, 
        int area, 
        string address, 
        int numberOfFloor, 
        int idSection,
        int idCity,
        int idProvince,
        bool isEarthSystem,
        bool isBuildingInspection,
        bool panelNeed,
        ErtTariff ertTariff,
        bool isNeedEb,
        bool hasRelatedPermit,
        bool hasSupervision,
        int areaAsBuilt
        )
    {
        LicenseNumber = licenseNumber;
        LandlordName = landlordName;
        CompanyName = companyName;
        LandlordNaCode = landlordNaCode;
        LandlordPhoneNumber = landlordPhoneNumber;
        PostalCode = postalCode;
        Area = area;
        Address = address;
        NumberOfFloor = numberOfFloor;
        IdSection = idSection;
        IdCity = idCity;
        IdProvince = idProvince;
        IsEarthSystem = isEarthSystem;
        IsBuildingInspection = isBuildingInspection;
        PanelNeed = panelNeed;
        ErtTariff = ertTariff;
        IsNeedEb = isNeedEb;
        HasRelatedPermit = hasRelatedPermit;
        HasSupervision = hasSupervision;
        AreaAsBuilt = areaAsBuilt;
    }
    public void UpdateWithElectCompany(
        string landlordName,
        string companyName,
        string landlordNaCode,
        string landlordPhoneNumber,
        string postalCode,
        string address,
        int numberOfFloor,
        int idSection,
        long electRequestNumber,
        bool  hasRelatedPermit
    )
    {
        LandlordName = landlordName;
        CompanyName = companyName;
        LandlordNaCode = landlordNaCode;
        LandlordPhoneNumber = landlordPhoneNumber;
        PostalCode = postalCode;
        Address = address;
        NumberOfFloor = numberOfFloor;
        IdSection = idSection;
        ElectRequestNumber = electRequestNumber;
        HasRelatedPermit = hasRelatedPermit;
    }

    public void SubmitByAdmin()
    {
        IsOk = true;
        SolarDateSendToElectCo = Helper.MiladiToShamsi(DateTime.Now.Date);
        JulianDateSendToElectCo = DateTime.Now.Date;
        ProjectLevelEnum = ProjectLevelEnum.EdcStage;
        ElectProjectStatusEnum = ElectProjectStatusEnum.DeliveredToEdc;
    }
    public void UpdateStatus(ElectProjectStatusEnum electProjectStatusEnum)
    {
        ElectProjectStatusEnum = electProjectStatusEnum;
    }
    public void SubmitTestAndDelivery()
    {
        IsOk = true;
    }
    public void StopProject(bool stop, string des)
    {
        IsStop = stop;
        StopDes = des;

    }
    public void SoftDelete()
    {
        IsDelete = true;
    }

    public void UpdateProjectLevel(ProjectLevelEnum projectLevel)
    {
        ProjectLevelEnum = projectLevel;
    }

    public void UpdateIsOk(bool isOk)
    {
        IsOk = isOk;
    }

    public void UpdateCancelStage(ProjectLevelEnum cancelStage)
    {
        ProjectLevelEnum = cancelStage;
    }

	public void CancelErtSystem()
	{
		IsEarthSystem = false;

	}

	public void UpdateExpertStage(ProjectLevelEnum expertStage)
    {
        ProjectLevelEnum = expertStage;

    }


    public void UpdateBuildingTypeEnum(BuildingTypeEnum buildingTypeEnum)
    {
        BuildingTypeEnum = buildingTypeEnum;
    }

    public void UpdatePanelMaker(PanelMaker panelMaker)
    {
        PanelMaker = panelMaker;
        JulianDatePanelRegister = DateTime.Now.Date;
        SolarDatePanelRegister = Helper.MiladiToShamsi(DateTime.Now.Date);
    }

    public void SubmitPanel(string panelSerialNumber,bool panelMakerSubmit)
    {
        PanelSerialNumber = panelSerialNumber;
        PanelMakerSubmit = panelMakerSubmit;
        JulianDatePanelSubmit = DateTime.Now.Date;
        SolarDatePanelSubmit = Helper.MiladiToShamsi(DateTime.Now.Date);
    }

    public void UpdateBuildingTariff(BuildingTariff buildingTariff)
    {
        BuildingTariff = buildingTariff;
    }

    public void UpdateNeedElectNetwork(bool isNeed)
    {
        NeedElectNetwork = isNeed;
    }

    public void UpdateCountSend(int count)
    {
        CountSendToElectCo = count;
    }

    public void UpdateSupervisor(string supervisorName,string supervisorPhoneNumber)
    {
        SupervisorName = supervisorName;
        SupervisorPhoneNumber = supervisorPhoneNumber;
    }

    public void UpdateErtTariff(ErtTariff ertTariff)
    {
        ErtTariff = ertTariff;
    }

    public void UpdateParentProject(ElectProject parentProject)
    {
        ParentProject = parentProject;
    }

    public void UpdateBigProject(long amountPerArea)
    {
        IsBigProject = true;
        AmountPerArea = amountPerArea;
    }

    public void UpdateDefectDes(string defectDes)
    {
        DefectDes = defectDes;
    }

    public void UpdateDefectByAdmin(string defectDes,bool solveDefectEng)
    {
        IsDefectEng = true;
        SolvedDefectEng = solveDefectEng;
        DefectAdminDes = defectDes;
        SolarDateDefectEng = Helper.MiladiToShamsi(DateTime.Now.Date);
        JulianDateDefectEng = DateTime.Now.Date;
    }
    public void UpdateSolvedDefectEng(string defectDes)
    {
        SolvedDefectEng = true;
        DefectEngDes = defectDes;
        SolarDateSolvedDefectEng = Helper.MiladiToShamsi(DateTime.Now.Date);
        JulianDateSolvedDefectEng = DateTime.Now.Date;
    }
}