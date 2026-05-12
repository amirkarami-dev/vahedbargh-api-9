using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using Microsoft.EntityFrameworkCore;

namespace Coreapi.Persistence.Repositories;

public class ElectProjectErtFormRepository:BaseRepository<ElectProjectErtForm>,IElectProjectErtFormRepository
{
    public ElectProjectErtFormRepository(CoreapiDbContext context) : base(context)
    {
    }

    public async Task<ElectProjectErtForm> GetEpeFormById(Guid id)
    {
        return await context.ElectProjectErtForms.Include((i => i.ElectProject)).FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<ElectProjectErtForm> GetEpeFormByElectProjectId(Guid electProjectId)
    {
        return await context.ElectProjectErtForms.Include(i => i.ElectProject)
            .FirstOrDefaultAsync(c => c.ElectProject.Id == electProjectId);
    }

    public async Task<IEnumerable<ElectProjectErtForm>> GetAllErtForm()
    {
        return await context.ElectProjectErtForms.Include(i => i.ElectProject).ToListAsync();
    }
}