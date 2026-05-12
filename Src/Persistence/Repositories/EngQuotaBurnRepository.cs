using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coreapi.Domain.AggregatesModel.FinanceAgg;
using Microsoft.EntityFrameworkCore;

namespace Coreapi.Persistence.Repositories;

public class EngQuotaBurnRepository(CoreapiDbContext context)
    : BaseRepository<EngQuotaBurn>(context), IEngQuotaBurnRepository
{
    public async Task<IEnumerable<EngQuotaBurn>> GetAllEngQuotaBurn(int fromPeriod)
    {
        return await context.EngQuotaBurns
            .Include(i=>i.Engineer)
            .Include(i=>i.QuarterTariff)
            .Where(i=>i.QuarterTariff.Period >= fromPeriod)
            .ToListAsync();
    }
    public async Task<IEnumerable<EngQuotaBurn>> GetAllEngQuotaBurn(bool approved, int fromPeriod)
    {
        return await context.EngQuotaBurns
            .Where(w=>w.Approved == approved)
            .Include(i => i.Engineer)
            .Include(i => i.QuarterTariff)
            .Where(i=>i.QuarterTariff.Period >= fromPeriod)
            .ToListAsync();
    }

    public async Task<IEnumerable<EngQuotaBurn>> GetAllEngQuotaBurnFromPeriod(bool approved, int period)
    {
        return await context.EngQuotaBurns
            .Where(w => w.Approved == approved)
            .Include(i => i.Engineer)
            .Include(i => i.QuarterTariff)
            .Where(w=>w.QuarterTariff.Period>=period)
            .ToListAsync();
    }
    public async Task<IEnumerable<EngQuotaBurn>> GetAllEngQuotaBurn(Guid? engId, Guid qtId)
    {

        return await context.EngQuotaBurns
            .Include(i => i.Engineer)
            .Include(i => i.QuarterTariff)
            .Where(w => engId == null || w.Engineer.Id == engId)
            .Where(w=> w.QuarterTariff.Id == qtId)
            .ToListAsync();

    }
}