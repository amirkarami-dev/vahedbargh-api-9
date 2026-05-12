using Microsoft.EntityFrameworkCore;

namespace Coreapi.Persistence
{
    public class CoreapiDbContextFactory : DesignTimeDbContextFactoryBase<CoreapiDbContext>
    {
        protected override CoreapiDbContext CreateNewInstance(DbContextOptions<CoreapiDbContext> options)
        {
            return new CoreapiDbContext(options);
        }
    }
}
