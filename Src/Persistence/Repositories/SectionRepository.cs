using System.Threading.Tasks;
using Coreapi.Domain.AggregatesModel.GeoAgg;
using Microsoft.EntityFrameworkCore;

namespace Coreapi.Persistence.Repositories;

public class SectionRepository(CoreapiDbContext context) : BaseRepository<Section>(context), ISectionRepository
{
    public async Task<Section> GetByIntId(int id)
    {
        return await context.Sections
            .Include(i=>i.City).FirstOrDefaultAsync(f => f.Id == id);
    }
}