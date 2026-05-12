using System.Collections.Generic;
using System.Threading.Tasks;
using Coreapi.Domain.SeedWork;

namespace Coreapi.Domain.AggregatesModel.QuarterIncrease;

public interface IQuarterIncreaseRepository:IRepository<QuarterIncrease>
{
    Task<IEnumerable<QuarterIncrease>> GetAllQuarterIncrease(int fromPeriod);
}