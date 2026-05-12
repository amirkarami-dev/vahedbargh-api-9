using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Common.Enums
{
    public enum InspectionStatusEnum
    {
        [Display(Name = "نامشخص")]
        Undefined = 0,
        [Display(Name = "تایید")]
        Done=1,
        [Display(Name = "عدم تایید")]
        Disapproval = 2,
        [Display(Name = "کنسل")]
        Canceled = 3


    }
}
