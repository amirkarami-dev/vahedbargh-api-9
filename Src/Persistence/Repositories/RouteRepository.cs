using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coreapi.Common.Utility;
using Coreapi.Domain.AggregatesModel.RoutAgg;
using Microsoft.EntityFrameworkCore;


namespace Coreapi.Persistence.Repositories;

public class RouteRepository:BaseRepository<Route>,IRoutRepository
{
    public RouteRepository(CoreapiDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Route>> GetAllRoutes()
    {
        List<Route> all = await context.Routes.Include(x => x.Parent).ToListAsync();
        ICollection<TreeExtensions.ITree<Route>> virtualRootNode = all.ToTree((parent, child) => child.ParentId == parent.Id).Children.ToList();
        var result = virtualRootNode.Select(c => c.Data);
 

        return result;
    }
  
}