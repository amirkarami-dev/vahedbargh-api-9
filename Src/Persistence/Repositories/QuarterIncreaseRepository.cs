using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coreapi.Domain.AggregatesModel.QuarterIncrease;
using Microsoft.EntityFrameworkCore;

namespace Coreapi.Persistence.Repositories;

public class QuarterIncreaseRepository(CoreapiDbContext context):BaseRepository<QuarterIncrease>(context), IQuarterIncreaseRepository
{
    public async Task<IEnumerable<QuarterIncrease>> GetAllQuarterIncrease(int fromPeriod)
    {
        return await context.QuarterIncreases
            .Include(i => i.Engineer)
            .Include(i => i.QuarterTariff)
            .Where(i=>i.QuarterTariff.Period >= fromPeriod)
            .ToListAsync();
    }
}