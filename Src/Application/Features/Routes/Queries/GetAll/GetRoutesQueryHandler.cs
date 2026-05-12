using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Coreapi.Domain.AggregatesModel.RoutAgg;
using MediatR;

namespace Coreapi.Application.Features.Routes.Queries.GetAll;

public class GetRoutesQueryHandler:IRequestHandler<GetRoutesQuery, IEnumerable<Route>>
{
    private readonly IRoutRepository routRepository;

    public GetRoutesQueryHandler(IRoutRepository routRepository)
    {
        this.routRepository = routRepository;
    }
    public async Task<IEnumerable<Route>> Handle(GetRoutesQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Route> routes;
        routes = await routRepository.GetAllRoutes();
        return routes;
    }
}