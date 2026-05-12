using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Domain.AggregatesModel.QuarterTariffAgg;
using Microsoft.EntityFrameworkCore;


namespace Coreapi.Persistence.Repositories
{
    public class QuarterTariffRepository(CoreapiDbContext context)
        : BaseRepository<QuarterTariff>(context), IQuarterTariffRepository
    {
		public async Task<IEnumerable<QuarterTariff>> GetGreateEqualThanPeriod(int period)
		{
			return await context.QuarterTariffs
            .OrderByDescending(c => c.Period)
            .Where(c=> c.Period >= period)
            .ToListAsync();
		}

		public async Task<QuarterTariff> GetLatest()
        {
            return await context.QuarterTariffs.OrderByDescending(c=>c.Period).FirstOrDefaultAsync();
        }

    }
}
