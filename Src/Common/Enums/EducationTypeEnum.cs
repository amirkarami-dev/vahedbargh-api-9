using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Common.Enums
{
    public enum EducationTypeEnum
    {
        [Display(Name = "کاردانی")]
        AssociateDegree = 1,
        [Display(Name = "کارشناسی")]
        BachelorDegree = 2,
        [Display(Name = "کارشناسی ارشد")]
        MasterDegree = 3,
        [Display(Name = "دکترا")]
        PhdDegree = 4,

    }
}
