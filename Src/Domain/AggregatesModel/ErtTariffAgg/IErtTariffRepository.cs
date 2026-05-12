using System.Threading.Tasks;
using Coreapi.Common.Enums;
using Coreapi.Domain.SeedWork;

namespace Coreapi.Domain.AggregatesModel.ErtTariffAgg;

public interface IErtTariffRepository:IRepository<ErtTariff>
{
    Task<ErtTariff> GetByType(ErtSystemTypeEnum  systemType);
}