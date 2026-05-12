using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.ViewModels;
using Coreapi.Domain.SeedWork;

namespace Coreapi.Domain.AggregatesModel.ExecutorAgg
{
    public interface IExecutorRepository:IRepository<Executor>
    {
        Task<Executor> GetExecutorByUserId(string userId);
        Task<IEnumerable<Executor>> GetByClientId(Guid clientId);
        Task<IEnumerable<ClientExecutorsViewModel>> GetByClientIdViewModel(Guid clientId);
        Task<Executor> GetExecutorByCellPhone(string cellPhone);
        Task<Executor> GetExecutorByNaCode(string naCode);
        Task<Executor> GetExecutorByMoreInfo(long moreInfo);
    }
}
