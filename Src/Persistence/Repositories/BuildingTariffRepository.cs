using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Enums;
using Coreapi.Domain.AggregatesModel.BuildingTariffAgg;
using Microsoft.EntityFrameworkCore;


namespace Coreapi.Persistence.Repositories
{
    public class BuildingTariffRepository(CoreapiDbContext context)
        : BaseRepository<BuildingTariff>(context), IBuildingTariffRepository
    {
        public async Task<BuildingTariff> GetByType(BuildingGroupTypeEnum groupType, BuildingGroupParameterTypeEnum groupParameterType)
        {
            return await context.BuildingTariffs
                .OrderByDescending(a => a.JulianDateExecute)
                .FirstOrDefaultAsync(x => x.BuildingGroupTypeEnum == groupType && x.BuildingGroupParameterTypeEnum == groupParameterType);
        }

    }
}
