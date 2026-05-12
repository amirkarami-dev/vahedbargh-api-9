using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coreapi.Infrastructure.Identity
{
    public class IdentityDbContextFactory : DesignTimeDbContextFactoryBase<IdentityAppDbContext>
    {


        protected override IdentityAppDbContext CreateNewInstance(DbContextOptions<IdentityAppDbContext> options,
            OperationalStoreOptionsMigrations operationalStoreOptions)
        {
            return new IdentityAppDbContext(options);

        }
    }
}
