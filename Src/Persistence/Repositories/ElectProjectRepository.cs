using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coreapi.Common.Enums;
using Coreapi.Common.RequestModel;
using Coreapi.Common.Utility;
using Coreapi.Common.ViewModels;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using Microsoft.EntityFrameworkCore;
using CheckListForm = Coreapi.Common.ViewModels.CheckListForm;
using CheckListEdc = Coreapi.Common.ViewModels.CheckListEdc;
using CommentEngForm = Coreapi.Common.ViewModels.CommentEngForm;


namespace Coreapi.Persistence.Repositories;

public class ElectProjectRepository(CoreapiDbContext context)
    : BaseRepository<ElectProject>(context), IElectProjectRepository
{
    public async Task<long> GetLastFileNumber(Guid clientId, int startNumber)
    {
        var row = await context.ElectProjects.Where(t => EF.Property<Guid>(t, "ClientId") == clientId)
			.Where(w => w.FileNumber.ToString().StartsWith(startNumber.ToString()))
		    .OrderByDescending(p => p.FileNumber).FirstOrDefaultAsync();

		return row?.FileNumber ?? long.Parse($"{startNumber}00000");
	}


	public async Task<long> CheckIndexElectRequestNumber(Guid clientId, long electRequestNumber)
    {
        var row = await context.ElectProjects.FirstOrDefaultAsync(t => EF.Property<Guid>(t, "ClientId") == clientId && t.ElectRequestNumber == electRequestNumber);

        return row?.FileNumber ?? 0;

    }

    public async Task<ElectProject> GetElectProjectByFileNumber(long fileNumber)
    {
        return await context.ElectProjects.Where(c => c.FileNumber == fileNumber).SingleOrDefaultAsync();
    }

    public async Task<ElectProject> GetElectProjectById(Guid id)
    {
        return await context.ElectProjects
            .Include(i=>i.Client)
            .Include(i=>i.BuildingTariff)
            .Include(i=>i.ErtTariff)
            .Include(i=>i.ChildProjects)
            .Include(i=>i.ParentProject)
            .Where(c => c.Id == id)
            .SingleOrDefaultAsync();

    }

    public async Task<long> GetProjectBalance(Guid id)
    {
        var projectBalanceIn = await context.Transactions
            .Where(t => t.ProjectId == id.ToString() && t.TransactionStatus == TransactionStatusEnum.In)
            .SumAsync(s => s.Amount);
        var projectBalanceOut = await  context.Transactions
            .Where(t => t.ProjectId == id.ToString() && t.TransactionStatus == TransactionStatusEnum.Out)
            .SumAsync(s => s.Amount);
       return projectBalanceIn - projectBalanceOut;
    }

    public async Task<List<ElectProjectFile>> GetFilesByFileNumber(long fileNumber)
    {
        return await context.ElectProjectFiles
            .Where(f => f.ElectProject.FileNumber == fileNumber)
            .ToListAsync();
    }

    public async Task<List<ElectProject>> GetElectProjectsByLandlordNaCode(string naCode)
    {
        return await context.ElectProjects
            .Where(p => !p.IsDelete && p.LandlordNaCode == naCode)
            .OrderByDescending(p => p.FileNumber)
            .ToListAsync();
    }

    public async Task<ElectProject?> GetElectProjectByElectRequestNumber(long electRequestNumber)
    {
        return await context.ElectProjects
            .Where(p => !p.IsDelete && p.ElectRequestNumber == electRequestNumber)
            .FirstOrDefaultAsync();
    }

    public async Task<ElectProjectViewMainModel> GetElectProjectsFullFilter(Guid clientId,Guid? panelMakerId, int idSection,
        ElectProjectsFullFilterModel filterModel)
    {

        var mainModel = new ElectProjectViewMainModel();
        var isFilterParent = Guid.TryParse(filterModel.ParentId, out var parentGuid);
        var baseQuery = context.ElectProjects

            .Where(w => !w.IsDelete)
            .Where(w => w.IsStop == filterModel.IsStop)
            //.Where(w => isFilterParent || !filterModel.FilterProjectLevel || w.ParentProject == null)
            .Where(w => !isFilterParent || w.ParentProject.Id == parentGuid)
            .Where(w => !filterModel.FilterIsFilter ||
                        (
                            w.IsBuildingInspection == filterModel.IsBuildingInspection &&
                            w.IsEarthSystem == filterModel.IsEarthSystem &&
                            w.IsTestAndDelivery == filterModel.IsTestAndDelivery
                        )
            )
            .Where(w => w.Client.Id == clientId)
            .Where(w => filterModel.IdCity == 0 || w.IdCity == filterModel.IdCity)
            .Where(w => panelMakerId == null || w.PanelMaker.Id == panelMakerId)
            // بر اساس فیلتر وضعیت پرونده اگر درخواست کننده شرکت توزیع
            // باشد ردش میکنه
            .Where(w => filterModel.IsElectRequest || !filterModel.FilterProjectLevel ||
                        w.ProjectLevelEnum == filterModel.ProjectLevelEnum)
            // بر اساس وضعیت شرکت توزیع
            //.Where(w => !filterModel.IsElectRequest || filterModel.ProjectLevelEnum == ProjectLevelEnum.EdcStage ||
            //            w.ProjectLevelEnum == ProjectLevelEnum.EdcStage)

            .Where(w => filterModel.ProjectLevelEnum != ProjectLevelEnum.EdcStage || (
                // اگر 8 ارسال شود این ها چک میشوند یعنی توزیع سرچ بزنه با لول 8
                w.ElectProjectStatusEnum == filterModel.ElectProjectStatusEnum && w.ElectRequestNumber != w.FileNumber))

            .Where(w => string.IsNullOrEmpty(filterModel.SolarRegisterDate) ||
                        w.SolarRegisterDate.Replace("/", "").Replace("-", "") == filterModel
                            .SolarRegisterDate.Replace("-", "").Replace("/", ""))


            .Where(w => string.IsNullOrEmpty(filterModel.LandLordName) ||
                        w.LandlordName.Contains(filterModel.LandLordName))


            .Where(w => filterModel.FileNumber == 0 || EF.Functions.Like((string)(object)w.FileNumber,
                $"%{filterModel.FileNumber}%"))

            .Where(w => filterModel.ElectRequestNumber == 0 || EF.Functions.Like((string)(object)w.ElectRequestNumber,
                $"%{filterModel.ElectRequestNumber}%"))

            .Where(w => filterModel.IdSection == 0 ||
                        w.IdSection == filterModel.IdSection);

        mainModel.CountRow = await baseQuery.CountAsync();

            var endResult =  baseQuery
                .Include(i => i.BuildingTariff)
                .Include(i => i.ErtTariff)
                .Include(i => i.PanelMaker)
                .Include(i => i.ChildProjects)
                .Include(i => i.ParentProject)
                .OrderByDescending(o => o.FileNumber)
                .Skip(filterModel.Page * filterModel.PageSize)
                .Take(filterModel.PageSize)
                .Select(x => new ElectProjectViewModel
                
                {
                    
                    Id = x.Id,
                    ParentId = x.ParentProject.Id,
                    ParentElectProject = x.ParentProject != null ? new ElectProjectParentModel
                    {
                        Id = x.ParentProject.Id,
                        FileNumber = x.ParentProject.FileNumber,
                        ElectRequestNumber = x.ParentProject.ElectRequestNumber,
                        LandlordName = x.ParentProject.LandlordName,
                        Address = x.ParentProject.Address,
                        Area = x.ParentProject.Area,
                        ProjectLevelEnum = x.ParentProject.ProjectLevelEnum,
                        ProjectLevelName = x.ParentProject.ProjectLevelEnum.GetDisplayName(),
                        SolarRegisterDate = x.ParentProject.SolarRegisterDate,
                        IsStop = x.ParentProject.IsStop,
                        ElectProjectStatusEnum = x.ParentProject.ElectProjectStatusEnum,
                        ElectProjectStatusName = x.ParentProject.ElectProjectStatusEnum.GetDisplayName()
                    } : null,
                    Children = Array.Empty<string>(),
                    IsBigProject = x.IsBigProject,
                    CountChildren = x.ChildProjects.Count,
                    LandlordName = x.LandlordName,
                    SupervisorName = x.SupervisorName,
                    SupervisorPhoneNumber = x.SupervisorPhoneNumber,
                    Address = x.Address,
                    PostalCode = x.PostalCode,
                    Area = x.Area,
                    NumberOfFloor = x.NumberOfFloor,
                    BuildingGroupTypeEnum = x.BuildingTariff != null?x.BuildingTariff.BuildingGroupTypeEnum:BuildingGroupTypeEnum.G0,
                    BuildingGroupTypeName = x.BuildingTariff != null ? 
                        x.BuildingTariff.BuildingGroupTypeEnum.GetDisplayName(): BuildingGroupTypeEnum.G0.GetDisplayName(),
                    BuildingGroupParameterTypeEnum = x.BuildingTariff != null ? x.BuildingTariff.BuildingGroupParameterTypeEnum:BuildingGroupParameterTypeEnum.B0,
                    BuildingGroupParameterTypeName = x.BuildingTariff != null ? 
                        x.BuildingTariff.BuildingGroupParameterTypeEnum.GetDisplayName(): BuildingGroupParameterTypeEnum.B0.GetDisplayName(),
                    ProjectCreatedTypeEnum = x.ProjectCreatedTypeEnum,
                    ProjectCreatedTypeName = x.ProjectCreatedTypeEnum.GetDisplayName(),
                    ProjectTypeRequestEnum = x.ProjectTypeRequestEnum,
                    ProjectTypeRequestName = x.ProjectTypeRequestEnum.GetDisplayName(),
                    ErtSystemTypeEnum = x.ErtTariff != null ? x.ErtTariff.ErtSystemTypeEnum:ErtSystemTypeEnum.None,
                    ErtSystemTypeName = x.ErtTariff != null? x.ErtTariff.ErtSystemTypeEnum.GetDisplayName():ErtSystemTypeEnum.None.GetDisplayName(),


                    ProjectLevelEnum = x.ProjectLevelEnum,
                    CompanyName = x.CompanyName,
                    CountSendToElectCo = x.CountSendToElectCo,
                    Description = x.Description,
                    Expired = x.Expired,
                    FileNumber = x.FileNumber,
                    ElectRequestNumber = x.ElectRequestNumber,
                    IsDelete = x.IsDelete,
                    JulianRegisterDate = x.JulianRegisterDate,
                    IdSection = x.IdSection,
                    IsOk = x.IsOk,
                    JulianDateDeliver = x.JulianDateDeliver,
                    PanelMakerSubmit = x.PanelMakerSubmit,
                    PanelNeed = x.PanelNeed,
                    PanelSerialNumber = x.PanelSerialNumber,
                    JulianDatePanelRegister = x.JulianDatePanelRegister,
                    SolarDatePanelRegister = x.SolarDatePanelRegister,
                    JulianDatePanelSubmit = x.JulianDatePanelSubmit,
                    SolarDatePanelSubmit = x.SolarDatePanelSubmit,
                    

                    ElectProjectProcessViewModel = context.ElectProjectProcesses.Include(g => g.ElectProject)
                        .Include(e => e.Engineer)
                        .Where(c => c.ElectProject.Id == x.Id && !c.IsDelete).Select(d =>
                            new ElectProjectProcessViewModel
                            {
                                Id = d.Id,
                                SolarDateDeliverOffice = d.SolarDateDeliverOffice,
                                Defect = d.Defect,
                                Description = d.Description,
                                SolarDateDeliverEngineer = d.SolarDateDeliverEngineer,
                                EngName = d.Engineer.FullName,
                                InspectionStatusEnum = d.InspectionStatusEnum,
                                CellPhone = d.Engineer.CellPhone,
                                ProjectLevelEnum = d.ProjectLevelEnum,
                                JulianDateDeliverEngineer = d.JulianDateDeliverEngineer,
                            }).OrderByDescending(o => o.JulianDateDeliverEngineer).ToList(),
                    Invoice = context.Invoices.Include(i => i.ElectProject)
                        .Include(i => i.Transaction)
                        .Where(w => w.ElectProject.Id == x.Id)
                        .Select(f => new InvoiceModel
                    {
                        Id = f.Id,
                        Amount = f.Amount,
                        InvoicePayType = f.InvoicePayType,
                        InvoicePayTypeName = f.InvoicePayType.GetDisplayName(),
                        InvoiceStatus = f.InvoiceStatus,
                        InvoiceStatusName = f.InvoiceStatus.GetDisplayName()
                    }).FirstOrDefault(),
                    LandlordNaCode = x.LandlordNaCode,
                    LandlordPhoneNumber = x.LandlordPhoneNumber,
                    Lat = x.Lat,
                    LicenseNumber = x.LicenseNumber,
                    Lng = x.Lng,
                    SolarRegisterDate = x.SolarRegisterDate,
                    UserId = x.UserId,
                    ProjectLevel = x.ProjectLevelEnum.GetDisplayName(),

                    IsUploadElectPlan = context.ElectProjectFiles.Include(i => i.ElectProject).Any(
                        f => f.ElectProject.Id == x.Id && f.FileTypeEnum == FileTypeEnum.ElectPlan
                    ),
                    IsUploadRelatedPermit = context.ElectProjectFiles.Include(i => i.ElectProject).Any(
                        f => f.ElectProject.Id == x.Id && f.FileTypeEnum == FileTypeEnum.RelatedPermit
                    ),
                    IsEarthSystem = x.IsEarthSystem,
                    IsErtTest = x.IsErtTest,
                    IsBuildingInspection = x.IsBuildingInspection,
                    IsTestAndDelivery = x.IsTestAndDelivery,
                    
                    IsStop = x.IsStop,
                    StopDes = x.StopDes,
                    ProjectBalanceIn = context.Transactions
                        .Where(t => t.ProjectId == x.Id.ToString() && t.TransactionStatus == TransactionStatusEnum.In)
                        .Sum(s => s.Amount),
                    ProjectBalanceOut = context.Transactions
                        .Where(t => t.ProjectId == x.Id.ToString() && t.TransactionStatus == TransactionStatusEnum.Out)
                        .Sum(s => s.Amount),
                    PanelMaker = !string.IsNullOrEmpty(x.PanelMaker.CompanyName) ? new PanelMakerModel()
                        {
                            Id = x.PanelMaker.Id,
                            CompanyName = x.PanelMaker.CompanyName,
                            FullName =  x.PanelMaker.FullName,
                        }:null,
                    CheckListForms = context.CheckListForms.Include(i => i.ElectProject).Where(w => w.ElectProject.Id == x.Id).Select(s => new CheckListForm()
                    {
                        Id = s.Id,
                        InspectionDesEnum = s.InspectionDesEnum,
                        InspectionDesName = Helper.GetCheckListEnum((int)s.InspectionDesEnum),
                        IsComplete = s.IsComplete,
                        ResultDes = s.ResultDes,
                        SolarChecked = s.SolarChecked,

                    }).ToList(),
                    CheckListEdcs = context.CheckListEdcs.Include(i => i.ElectProject).Where(w => w.ElectProject.Id == x.Id).Select(s => new CheckListEdc()
                    {
                        Id = s.Id,
                        CheckListEdcEnum = s.CheckListEdcEnum,
                        CheckListEdcName = Helper.GetCheckListEnum((int)s.CheckListEdcEnum),
                        IsComplete = s.IsComplete,
                        ResultDes = s.ResultDes,
                        SolarChecked = s.SolarChecked,

                    }).ToList(),
                    CommentEngForms = context.CommentEngForms.Include(i => i.ElectProject).Where(w => w.ElectProject.Id == x.Id).Select(s => new CommentEngForm
                    {
                        Ampere = s.Ampere,
                        BranchingCount = s.BranchingCount,
                        BranchingTypeEnum = s.BranchingTypeEnum,
                        BranchingTypeName = s.BranchingTypeEnum.GetDisplayName(),
                        Des = s.Des,
                        FazNumberEnum = s.FazNumberEnum,
                        FazNumberName = s.FazNumberEnum.GetDisplayName(),
                        Id = s.Id,
                        Power = s.Power,
                        PowerSum = s.PowerSum,
                    }).ToList(),
                    JulianDateSendToElectCo = x.JulianDateSendToElectCo,
                    SolarDateSendToElectCo = x.SolarDateSendToElectCo,
                    AmountPerArea = x.AmountPerArea,
                    foundationElectrodeArea = x.FoundationElectrodeArea,
                    isNeedEb = x.IsNeedEb,
                    ElectProjectStatusEnum = x.ElectProjectStatusEnum,
                    ElectProjectStatusName = x.ElectProjectStatusEnum.GetDisplayName(),
                    DefectDes = x.DefectDes,
                    IsDefectEng= x.IsDefectEng,
                    SolvedDefectEng=x.SolvedDefectEng,
                    JulianDateDefectEng=x.JulianDateDefectEng,
                    SolarDateDefectEng=x.SolarDateDefectEng,
                    JulianDateSolvedDefectEng=x.JulianDateSolvedDefectEng,
                    SolarDateSolvedDefectEng=x.SolarDateSolvedDefectEng,
                    DefectEngDes=x.DefectEngDes,
                    DefectAdminDes=x.DefectAdminDes,
                    HasRelatedPermit = x.HasRelatedPermit,
                    HasSupervision = x.HasSupervision,
                    AreaAsBuilt = x.AreaAsBuilt,
					NeedElectNetwork = x.NeedElectNetwork

				}
            );

            mainModel.ElectProjectViewModel = await endResult.ToListAsync();


        return mainModel;
    }


}