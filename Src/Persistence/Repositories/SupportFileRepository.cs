using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coreapi.Domain.AggregatesModel.SupportAgg;
using Microsoft.EntityFrameworkCore;

namespace Coreapi.Persistence.Repositories;

public class SupportFileRepository:BaseRepository<SupportFile>, ISupportFileRepository
{
    public SupportFileRepository(CoreapiDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<SupportFile>> GetBySupportId(Guid supportId)
    {
        return await context.SupportFiles.Include(i=>i.Support).Where(w=>w.Support.Id == supportId).ToListAsync();
    }

    public void DeleteSupportFile(SupportFile file)
    {
       context.SupportFiles.Remove(file);
    }
}