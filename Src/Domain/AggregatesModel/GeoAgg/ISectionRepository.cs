using System.Threading.Tasks;
using Coreapi.Domain.SeedWork;

namespace Coreapi.Domain.AggregatesModel.GeoAgg;

public interface ISectionRepository:IRepository<Section>
{
    Task<Section> GetByIntId(int id);
}