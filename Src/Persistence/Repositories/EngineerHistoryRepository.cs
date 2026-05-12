using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Domain.AggregatesModel.EngineerAgg;
using Microsoft.EntityFrameworkCore;


namespace Coreapi.Persistence.Repositories
{
    public class EngineerHistoryRepository:BaseRepository<EngineerHistory>,IEngineerHistoryRepository
    {
        public EngineerHistoryRepository(CoreapiDbContext context) : base(context)
        {
        }

        public async Task<EngineerHistory> GetByEngId(Guid engId)
        {
            return await context.EngineerHistories.Include(e => e.Engineer)
                .FirstOrDefaultAsync(f => f.Engineer.Id == engId);
        }
    }
}
