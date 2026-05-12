using Coreapi.Common.Enums;
using Coreapi.Domain.AggregatesModel.BuildingTariffAgg;
using Coreapi.Domain.AggregatesModel.ErtTariffAgg;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Coreapi.Persistence.Repositories;

public class ErtTariffRepository:BaseRepository<ErtTariff>, IErtTariffRepository
{
    public ErtTariffRepository(CoreapiDbContext context) : base(context)
    {
    }

    public async Task<ErtTariff> GetByType(ErtSystemTypeEnum ertSystemType)
    {
        return await context.ErtTariffs.OrderByDescending(a => a.JulianDateExecute)
            .FirstOrDefaultAsync(x => x.ErtSystemTypeEnum == ertSystemType);
    }
}