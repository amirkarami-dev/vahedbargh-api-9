using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Coreapi.Common.Utility;


namespace Coreapi.Persistence.Repositories;

public class CheckListFormRepository:BaseRepository<CheckListForm>,ICheckListFormRepository
{
    public CheckListFormRepository(CoreapiDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<CheckListForm>> GetAllCheckListForm()
    {
        return await context.CheckListForms.Include(i => i.ElectProject).Include(i => i.Engineer).ToListAsync();

    }

    public async Task<IEnumerable<CheckListForm>> GetAllCheckListForm(Guid engId)
    {
        return await context.CheckListForms
            .Include(i => i.ElectProject)
            .Include(i => i.Engineer)
            .Where(w=>w.Engineer.Id == engId)
            .ToListAsync();

    }

    public async Task<IEnumerable<Common.ViewModels.CheckListForm>> GetCheckLIstEngForm(Guid electProjectId, Guid engId)
    {
        return await context.CheckListForms
            .Include(i => i.ElectProject)
            .Include(i => i.Engineer)
            .Where(w => w.Engineer.Id == engId)
            .Where(w => w.ElectProject.Id == electProjectId)
            .Select(s => new Common.ViewModels.CheckListForm()
            {
                Id = s.Id,
                InspectionDesEnum = s.InspectionDesEnum,
                InspectionDesName = Helper.GetInspectionDesEnum((int)s.InspectionDesEnum),
                IsComplete = s.IsComplete,
                ResultDes = s.ResultDes,
                SolarChecked = s.SolarChecked,
            })
            .ToListAsync();
    }
}