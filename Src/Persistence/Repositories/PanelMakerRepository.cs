using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coreapi.Domain.AggregatesModel.PanelMakerAgg;
using Microsoft.EntityFrameworkCore;

namespace Coreapi.Persistence.Repositories;

public class PanelMakerRepository:BaseRepository<PanelMaker>,IPanelMakerRepository
{
    public PanelMakerRepository(CoreapiDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<PanelMaker>> GetClientPanelMakers(Guid clientId)
    {
        return await context.PanelMakers.Include(i=>i.Client).Where(w=>w.Client.Id == clientId).ToListAsync();
    }

    public async Task<PanelMaker> GetByUserid(string userid)
    {
        return await context.PanelMakers.FirstOrDefaultAsync(f => f.UserId == userid);
    }
}