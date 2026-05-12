using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Common.Enums
{
    public enum RelatedTypeEnum
    {
        [Display(Name = "اصلی")]
        Primary = 1,
        [Display(Name = "مرتبط")]
        Related = 2

    }
}
