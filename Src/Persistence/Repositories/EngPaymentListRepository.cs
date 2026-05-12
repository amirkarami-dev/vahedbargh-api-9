using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coreapi.Domain.AggregatesModel.FinanceAgg;
using Microsoft.EntityFrameworkCore;


namespace Coreapi.Persistence.Repositories;

public class EngPaymentListRepository:BaseRepository<EngPaymentList>, IEngPaymentListRepository
{
    public EngPaymentListRepository(CoreapiDbContext context) : base(context)
    {
    }



    public async Task<IEnumerable<EngPaymentList>> GetEngPaymentList(Guid engPaymentTaskId)
    {
       var engPaymentList = await context.EngPaymentLists
           .Include(i=>i.Transaction)
           .Include(i=>i.Engineer)
           .Include(i=>i.EngPaymentTask)
           .Where(w=>w.EngPaymentTask.Id == engPaymentTaskId).OrderBy(o=>o.Engineer.SortIndex)
           .ToListAsync();
       return engPaymentList;
    }

    public async Task<EngPaymentList> GetEngPaymentListById(Guid id)
    {
        var engPaymentList = await context.EngPaymentLists
            .Include(i => i.Transaction)
            .Include(i => i.Engineer)
            .Include(i => i.EngPaymentTask)
            .Where(w => w.Id == id).FirstOrDefaultAsync();
        return engPaymentList;
    }
}