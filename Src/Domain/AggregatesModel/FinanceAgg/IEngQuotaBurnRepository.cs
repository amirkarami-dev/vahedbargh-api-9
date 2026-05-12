using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coreapi.Domain.SeedWork;

namespace Coreapi.Domain.AggregatesModel.FinanceAgg;

public interface IEngQuotaBurnRepository:IRepository<EngQuotaBurn>
{
    Task<IEnumerable<EngQuotaBurn>> GetAllEngQuotaBurn(int fromPerid);
    Task<IEnumerable<EngQuotaBurn>> GetAllEngQuotaBurn(bool approved, int fromPeriod);
    Task<IEnumerable<EngQuotaBurn>> GetAllEngQuotaBurnFromPeriod(bool approved, int period);
    Task<IEnumerable<EngQuotaBurn>> GetAllEngQuotaBurn(Guid? engId, Guid qtId);
}