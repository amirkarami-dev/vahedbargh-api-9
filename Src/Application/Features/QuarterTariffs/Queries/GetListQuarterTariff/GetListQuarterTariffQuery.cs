using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Domain.AggregatesModel.QuarterTariffAgg;
using MediatR;

namespace Coreapi.Application.Features.QuarterTariffs.Queries.GetListQuarterTariff
{
    public class GetListQuarterTariffQuery:IRequest<IEnumerable<ListQuarterTariffDto>>
    {
    }
}
