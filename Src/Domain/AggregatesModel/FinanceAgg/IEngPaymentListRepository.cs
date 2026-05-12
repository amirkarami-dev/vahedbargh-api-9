using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coreapi.Domain.SeedWork;

namespace Coreapi.Domain.AggregatesModel.FinanceAgg;

public interface IEngPaymentListRepository :IRepository<EngPaymentList>
{
    Task<IEnumerable<EngPaymentList>> GetEngPaymentList(Guid engPaymentTaskId);
    Task<EngPaymentList> GetEngPaymentListById(Guid id);
}