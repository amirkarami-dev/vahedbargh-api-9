using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Enums;
using Coreapi.Common.ViewModels;
using Coreapi.Domain.AggregatesModel.QuarterTariffAgg;
using Coreapi.Domain.SeedWork;

namespace Coreapi.Domain.AggregatesModel.EngineerAgg
{
    public interface IEngineerRepository:IRepository<Engineer>
    {
        Task<Engineer> getByUserId(string userId);
        Task<Engineer> getByNaCode(string naCode);
        long GetSumAmountQuota(long defaultQuota, IEnumerable<EngineerHistoryViewModel> historyViewModels,IEnumerable<QuarterTariff> quarterTariffs);

        long GetCountErtQuota(long defaultQuota, IEnumerable<EngineerHistoryViewModel> historyViewModels, IEnumerable<QuarterTariff> quarterTariffs);

        long GetSumAmountQuotaSingle(long defaultQuota, EngineerGradeTypeEnum engGradeTypeEnum, DateTime julianWorkPermit, QuarterTariff quarterTariff, WorkPermitTypeEnum workPermitTypeEnum);

        Task<IEnumerable<ClientEngineersViewModel>> GetByClientIdViewModel(Guid clientId, Guid? engId,  FilterCertEnum filterCertEnum);
        Task<IEnumerable<ClientEngWorkViewModel>> GetEngWorkViewModel(Guid clientId, Guid? engId, Guid idQuarterTariff,int period,int month1, int month2, int month3);
        Task<long> GetLastWorkPermitNum(Guid engId);
        Task<bool> CanDoThisProcess(Guid engId,Guid electProjectId);

        double GetEngFactor(EngineerHistoryViewModel historyViewModels);
    }
}
