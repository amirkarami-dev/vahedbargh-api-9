using System.Collections.Generic;
using Coreapi.Domain.AggregatesModel.RoutAgg;
using MediatR;

namespace Coreapi.Application.Features.Routes.Queries.GetAll
{
    public class GetRoutesQuery:IRequest<IEnumerable<Route>>
    {
    }
}
