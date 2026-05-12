using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Domain.SeedWork;

namespace Coreapi.Domain.AggregatesModel.QuarterTariffAgg
{
    public interface IQuarterTariffRepository:IRepository<QuarterTariff>

    {
        Task<QuarterTariff> GetLatest();
		Task<IEnumerable<QuarterTariff>> GetGreateEqualThanPeriod(int period);
	}
}
