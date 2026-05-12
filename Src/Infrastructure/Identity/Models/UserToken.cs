using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Enums;

namespace Coreapi.Infrastructure.Identity.Models
{
    public class UserToken
    {
        public string Token { get; set; }
        public PlatformTypeEnum Platform { get; set; }
    }
}
