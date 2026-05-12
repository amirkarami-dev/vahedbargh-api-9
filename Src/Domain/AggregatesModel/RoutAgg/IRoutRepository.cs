using System.Collections.Generic;
using System.Threading.Tasks;
using Coreapi.Domain.SeedWork;

namespace Coreapi.Domain.AggregatesModel.RoutAgg;

public interface IRoutRepository:IRepository<Route>
{
    Task<IEnumerable<Route>> GetAllRoutes();



}