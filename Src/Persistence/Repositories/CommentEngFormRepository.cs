using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coreapi.Common.Utility;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using Microsoft.EntityFrameworkCore;


namespace Coreapi.Persistence.Repositories;

public class CommentEngFormRepository:BaseRepository<CommentEngForm>, ICommentEngFormRepository
{
    public CommentEngFormRepository(CoreapiDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<CommentEngForm>> GetAllCommentEngForm()
    {
        return await context.CommentEngForms.Include(i=>i.ElectProject).Include(i=>i.Engineer).ToListAsync();
    }
    public async Task<IEnumerable<CommentEngForm>> GetAllCommentEngForm(Guid engId)
    {
        return await context.CommentEngForms
            .Include(i => i.ElectProject)
            .Include(i => i.Engineer)
            .Where(w=>w.Engineer.Id == engId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Common.ViewModels.CommentEngForm>> GetCommentEngForm(Guid electProjectId, Guid engId)
    {
        return await context.CommentEngForms
            .Include(i=>i.ElectProject)
            .Include(i=>i.Engineer)
            .Where(w=>w.Engineer.Id == engId)
            .Where(w=>w.ElectProject.Id == electProjectId)
            .Select(s=>new Common.ViewModels.CommentEngForm()
            {
                Ampere = s.Ampere,
                BranchingCount = s.BranchingCount,
                BranchingTypeEnum = s.BranchingTypeEnum,
                BranchingTypeName = s.BranchingTypeEnum.GetDisplayName(),
                Des = s.Des,
                FazNumberEnum = s.FazNumberEnum,
                FazNumberName = s.FazNumberEnum.GetDisplayName(),
                Id = s.Id,
                Power = s.Power,
                PowerSum = s.PowerSum
            })
            .ToListAsync();

    }
} 