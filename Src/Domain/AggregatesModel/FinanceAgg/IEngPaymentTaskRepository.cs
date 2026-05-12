using System.Collections.Generic;
using Coreapi.Domain.SeedWork;
using System.Threading.Tasks;

namespace Coreapi.Domain.AggregatesModel.FinanceAgg;

public interface IEngPaymentTaskRepository:IRepository<EngPaymentTask>
{
    Task<int> GetLatestPeriod();
    Task<IEnumerable<EngPaymentTask>> GetByOrderPeriod();
    Task<EngPaymentTask> GetLastByOrderPeriod();
}