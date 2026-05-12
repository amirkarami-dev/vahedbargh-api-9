using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Coreapi.Common.Enums
{
    public enum PeriodEnum
    {
        [Display(Name = "اول")]
        I = 1,
        [Display(Name = "دوم")]
        II = 2,
        [Display(Name = "سوم")]
        III = 3,
        [Display(Name = "چهارم")]
        IV = 4,
    }
}
