using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Enums;
using Coreapi.Common.RequestModel;
using Coreapi.Common.ViewModels;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using Coreapi.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;


namespace Coreapi.Persistence.Repositories
{
    public class ElectProjectProcessRepository(CoreapiDbContext context)
		: BaseRepository<ElectProjectProcess>(context), IElectProjectProcessRepository
    {
        public async Task<IEnumerable<ElectProjectProcess>> GetElectProjectProcessByEpId(Guid electProjectId)
        {
            return await context.ElectProjectProcesses
                .Include(g => g.ElectProject)
                .Include(e=>e.Engineer)
                .OrderByDescending(o => o.JulianDateDeliverEngineer)
                .Where(c => c.ElectProject.Id == electProjectId && !c.IsDelete).ToListAsync();
        }

        public async Task<ElectProjectProcess> GetElectProjectProcessById(Guid gppId)
        {
            return await context.ElectProjectProcesses
                .Include(g => g.ElectProject)
                .Include(e => e.Engineer)
                .Include(i=>i.QuarterTariff)
                .FirstOrDefaultAsync(c => c.Id == gppId);
        }

        public async Task<IEnumerable<ElectProjectProcessFeeViewModel>> GetElectProjectProcessFee(int fromPeriod)
        {
            var query = await context.ElectProjectProcesses
                .Include(a => a.Engineer)
                .Include(i => i.QuarterTariff)
                .Where(w=>!w.IsDelete)
                .Where(w=> w.QuarterTariff.Period >= fromPeriod)
                .GroupBy(b => b.Engineer.Id)
                .Select(c => new ElectProjectProcessFeeViewModel()
                {
                    EngId = c.Key,
                    Fee = c.Where(d=>d.InspectionStatusEnum != InspectionStatusEnum.Canceled).Sum(e => e.Fee),
                    CountCancel = c.Count(d => d.InspectionStatusEnum == InspectionStatusEnum.Canceled),
                    CountExpertCancel = c.Count(d => d.ProjectLevelEnum == ProjectLevelEnum.ExpertStage && d.InspectionStatusEnum == InspectionStatusEnum.Canceled),
                    CountErtCancel = c.Count(d => d.ProjectLevelEnum == ProjectLevelEnum.ErtStage && d.InspectionStatusEnum == InspectionStatusEnum.Canceled),

                    CountErtProcess = c.Count(d=>d.ProjectLevelEnum == ProjectLevelEnum.ErtStage && d.InspectionStatusEnum != InspectionStatusEnum.Canceled),

                    CountErtProcessE6 = c.Count(d=>d.ProjectLevelEnum == 
                                                   ProjectLevelEnum.ErtStage && d.InspectionStatusEnum != InspectionStatusEnum.Canceled &&
                                                   d.ElectProject.ErtTariff.ErtSystemTypeEnum == ErtSystemTypeEnum.E6 &&
                                                   d.JulianRegisterDate >= DateTime.Parse("2025-04-21").Date),

                    SumAreaTestAndDelivery = c.Where(w=>w.ElectProject.IsTestAndDelivery && w.InspectionStatusEnum != InspectionStatusEnum.Canceled).Sum(d=>d.ElectProject.Area)
                }).ToListAsync();
            return query;
        }

        public async Task<IEnumerable<ElectProjectProcessFeeViewModel>> GetElectProjectProcessFee(Guid qtId)
        {
            var query = await context.ElectProjectProcesses.Include(a => a.Engineer)
                .Where(w => !w.IsDelete)
                .Where(w => w.QuarterTariff.Id == qtId)
                .GroupBy(b => b.Engineer.Id)
                .Select(c => new ElectProjectProcessFeeViewModel()
                {
                    EngId = c.Key,
                    Fee = c.Where(d => d.InspectionStatusEnum != InspectionStatusEnum.Canceled).Sum(e => e.Fee)
                }).ToListAsync();
            return query;
        }
        public async Task<IEnumerable<ElectProjectProcessFeeViewModel>> GetElectProjectProcessFeeBeforePeriod(int period)
        {
            var query = await context.ElectProjectProcesses.Include(a => a.Engineer)
                .Where(w => !w.IsDelete)
                .Where(w => w.QuarterTariff.Period < period)
                .GroupBy(b => b.Engineer.Id)
                .Select(c => new ElectProjectProcessFeeViewModel()
                {
                    EngId = c.Key,
                    Fee = c.Where(d => d.InspectionStatusEnum != InspectionStatusEnum.Canceled).Sum(e => e.Fee)
                }).ToListAsync();
            return query;
        }

        public async Task<BaseModelWithTotalRow<ElectProjectProcess>> GetEppByEng(Guid idEngineer, EppFilterModel eppFilterModel)
        {
            var mainModel = new BaseModelWithTotalRow<ElectProjectProcess>();
            var listEpp = context.ElectProjectProcesses
                .Include(i => i.ElectProject)
                .Include(i => i.QuarterTariff)
                .Include(i => i.Engineer)

                .Where(w => !w.IsDelete)
                .Where(w=> w.InspectionStatusEnum == eppFilterModel.InspectionStatusEnum)
                .Where(w => string.IsNullOrEmpty(eppFilterModel.SolarDateDeliverEngineer) || w.SolarDateDeliverEngineer.Replace("/", "").Replace("-", "") == eppFilterModel.SolarDateDeliverEngineer.Replace("-", "").Replace("/", ""))
                .Where(w => eppFilterModel.FileNumber == 0 || w.ElectProject.FileNumber == eppFilterModel.FileNumber)
                .Where(w => w.Engineer.Id == idEngineer)
                .Where(w => eppFilterModel.IdSection == 0 || w.ElectProject.IdSection == eppFilterModel.IdSection)
                .Where(w => eppFilterModel.FileNumber > 0 || w.InspectionStatusEnum == eppFilterModel.InspectionStatusEnum)
                .OrderByDescending(o=>o.JulianDateDeliverEngineer);

                var endResult = await listEpp.ToListAsync();
                var endResultWithPagination = endResult
                    .Skip(eppFilterModel.Page * eppFilterModel.PageSize).Take(eppFilterModel.PageSize);


                mainModel.AggregateModel = endResultWithPagination;
                mainModel.TotalItem = endResult.Count;

            return mainModel;
        }

        public async Task<BaseModelWithTotalRow<ElectProjectProcess>> GetEppByClient(Guid clientId, EppFilterModel eppFilterModel)
        {

            var mainModel = new BaseModelWithTotalRow<ElectProjectProcess>();

            var listEpp = context.ElectProjectProcesses
                .Include(i => i.Client)
                .Include(i => i.ElectProject)
                .Include(i => i.ElectProject.ParentProject)
				.Include(i => i.QuarterTariff)
                .Include(i => i.Engineer)
                .Where(w => !w.IsDelete)
                .Where(w => string.IsNullOrEmpty(eppFilterModel.EngineerId) ||
                            w.Engineer.Id == Guid.Parse(eppFilterModel.EngineerId))
                .Where(w => eppFilterModel.InspectionStatusEnum == InspectionStatusEnum.Undefined ||
                            w.InspectionStatusEnum == eppFilterModel.InspectionStatusEnum)
                .Where(w => string.IsNullOrEmpty(eppFilterModel.SolarDateDeliverEngineer) ||
                            w.SolarDateDeliverEngineer.Replace("/", "").Replace("-", "") == eppFilterModel
                                .SolarDateDeliverEngineer.Replace("-", "").Replace("/", ""))
                .Where(w => string.IsNullOrEmpty(eppFilterModel.LandlordName) ||
                            w.ElectProject.LandlordName.Contains(eppFilterModel.LandlordName))
                .Where(w => eppFilterModel.FileNumber == 0 || w.ElectProject.FileNumber == eppFilterModel.FileNumber)
                .Where(w => w.Client.Id == clientId)
                .Where(w => eppFilterModel.IdSection == 0 || w.ElectProject.IdCity == eppFilterModel.IdSection)
                .Where(w => eppFilterModel.FileNumber > 0 || w.InspectionStatusEnum == eppFilterModel.InspectionStatusEnum)

                .OrderByDescending(o => o.ElectProject.FileNumber);


                var endResult = await listEpp.ToListAsync();
                var endResultWithPagination = endResult
                    .Skip(eppFilterModel.Page * eppFilterModel.PageSize).Take(eppFilterModel.PageSize);

                mainModel.AggregateModel = endResultWithPagination;
                mainModel.TotalItem = endResult.Count;

          return mainModel;

        }

        public async Task<IEnumerable<ElectProjectProcess>> GetEppNotAccepted(int day)
        {
            var currentDate = DateTime.Now;
            var listEpp = await context.ElectProjectProcesses
                .Include(i => i.ElectProject)
                .Where(w => !w.ElectProject.IsDelete)
                .Where(w => !w.IsDelete)
                .Where(w=> !w.Accepted)
                .Where(w=> w.ProjectLevelEnum != ProjectLevelEnum.ApproveErtStage)
                .Where(w=> w.ProjectLevelEnum != ProjectLevelEnum.ErtStage)
                .Where(w=>w.InspectionStatusEnum != InspectionStatusEnum.Canceled)
                .ToListAsync();

            return listEpp.Where(e => (currentDate - e.JulianDateDeliverEngineer).TotalDays > day); ;

        }

        public async Task<IEnumerable<ElectProjectProcess>> GetEppByApproved(Guid epId)
        {
            return await context.ElectProjectProcesses
                .Include(g => g.ElectProject)
                .Include(e => e.Engineer)
                .Include(i => i.QuarterTariff)
                .Where(c => c.ElectProject.Id == epId && c.InspectionStatusEnum ==InspectionStatusEnum.Done)
                .ToListAsync();
        }

	}



}


