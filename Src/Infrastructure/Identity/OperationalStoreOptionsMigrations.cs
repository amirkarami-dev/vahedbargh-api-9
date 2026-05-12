using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Coreapi.Infrastructure.Identity
{
    public class OperationalStoreOptionsMigrations :
    IOptions<StoreOptions>
    {
        public StoreOptions Value => new ()
        {


        };
    }
}
