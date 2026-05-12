using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using System.Linq;
using Coreapi.Common.Utility;
using Microsoft.EntityFrameworkCore;

namespace Coreapi.Persistence.Repositories;

public class CheckListEdcRepository:BaseRepository<CheckListEdc>,ICheckListEdcRepository
{
    public CheckListEdcRepository(CoreapiDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<CheckListEdc>> GetAllCheckLisEdc()
    {
        return await context.CheckListEdcs.Include(i => i.ElectProject).ToListAsync();
    }


    public async Task<IEnumerable<Common.ViewModels.CheckListEdc>> GetCheckListEdcForm(Guid electProjectId)
    {
        return await context.CheckListEdcs
            .Include(i => i.ElectProject)
            .Where(w => w.ElectProject.Id == electProjectId)
            .Select(s => new Common.ViewModels.CheckListEdc()
            {
                Id = s.Id,
                CheckListEdcEnum = s.CheckListEdcEnum,
                CheckListEdcName = Helper.GetCheckListEnum((int)s.CheckListEdcEnum),
                IsComplete = s.IsComplete,
                ResultDes = s.ResultDes,
                SolarChecked = s.SolarChecked,
            })
            .ToListAsync();
    }
}