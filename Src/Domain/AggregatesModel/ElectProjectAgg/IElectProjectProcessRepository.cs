using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coreapi.Common.Enums;
using Coreapi.Common.RequestModel;
using Coreapi.Common.ViewModels;
using Coreapi.Domain.SeedWork;

namespace Coreapi.Domain.AggregatesModel.ElectProjectAgg
{
    public interface IElectProjectProcessRepository : IRepository<ElectProjectProcess>
    {
        Task<IEnumerable<ElectProjectProcess>> GetElectProjectProcessByEpId(Guid electProjectId);
        Task<ElectProjectProcess> GetElectProjectProcessById(Guid gppId);
        Task<IEnumerable<ElectProjectProcessFeeViewModel>> GetElectProjectProcessFee(int fromPeriod);
        Task<IEnumerable<ElectProjectProcessFeeViewModel>> GetElectProjectProcessFee(Guid qtId);
        Task<IEnumerable<ElectProjectProcessFeeViewModel>> GetElectProjectProcessFeeBeforePeriod(int period);
        Task<BaseModelWithTotalRow<ElectProjectProcess>> GetEppByEng(Guid idEngineer, EppFilterModel eppFilterModel);

        Task<BaseModelWithTotalRow<ElectProjectProcess>> GetEppByClient(Guid clientId, EppFilterModel eppFilterModel);

        Task<IEnumerable<ElectProjectProcess>> GetEppNotAccepted(int day); 
        Task<IEnumerable<ElectProjectProcess>> GetEppByApproved(Guid epId); 






    }
}
