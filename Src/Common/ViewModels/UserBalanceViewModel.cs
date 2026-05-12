using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Enums;

namespace Coreapi.Common.ViewModels
{
    public class UserBalanceViewModel
    {
        public string UserId { get; set; }
        public long Balance { get; set; }
    }
}
