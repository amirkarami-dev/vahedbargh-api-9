using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Enums;
using Coreapi.Domain.SeedWork;

namespace Coreapi.Domain.AggregatesModel.BuildingTariffAgg
{
    public interface IBuildingTariffRepository:IRepository<BuildingTariff>
    {
        Task<BuildingTariff> GetByType(BuildingGroupTypeEnum groupType, BuildingGroupParameterTypeEnum groupParameterType);
    }
}
