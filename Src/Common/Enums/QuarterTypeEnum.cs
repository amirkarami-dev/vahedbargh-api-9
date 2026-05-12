using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Common.Enums
{
    public enum QuarterTypeEnum
    {
        [Display(Name = "سه ماهه اول")]
        FirstTrimester=1,
        [Display(Name = "سه ماهه دوم")]
        SecondTrimester=2,
        [Display(Name = "سه ماهه سوم")]
        ThirdTrimester=3,
        [Display(Name = "سه ماهه چهارم")]
        FourthTrimester=4


    }
}
