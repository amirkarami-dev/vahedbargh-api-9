using System;
using System.Collections.Generic;
using Coreapi.Domain.AggregatesModel.FinanceAgg;

using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Coreapi.Persistence.Repositories;

public class EngPaymentTaskRepository(CoreapiDbContext context)
    : BaseRepository<EngPaymentTask>(context), IEngPaymentTaskRepository
{
    public async Task<int> GetLatestPeriod()
    {
        var period = await context.EngPaymentTasks.OrderByDescending(c => c.Period).FirstOrDefaultAsync();
        return period?.Period ?? 0;
    }

    public async Task<IEnumerable<EngPaymentTask>> GetByOrderPeriod()
    {
        return await context.EngPaymentTasks.OrderByDescending(c => c.Period).ToListAsync();
    }

    public async Task<EngPaymentTask> GetLastByOrderPeriod()
    {
        return await context.EngPaymentTasks.OrderByDescending(c => c.Period).FirstOrDefaultAsync();
    }
}