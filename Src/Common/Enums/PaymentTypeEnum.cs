using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Common.Enums
{
    public enum PaymentTypeEnum
    {
        [Display(Name = "آنلاین")]
        Online =1,
        [Display(Name = "فیش")]
        Manual=2
    }
}
