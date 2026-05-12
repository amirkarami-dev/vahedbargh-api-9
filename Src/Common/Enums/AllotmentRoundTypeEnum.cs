using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Common.Enums
{
    public enum AllotmentRoundTypeEnum
    {
        [Display(Name = "نوبت اول")]
        FirstRound=1,
        [Display(Name = "نوبت دوم")]
        SecondRound=2,
        [Display(Name = "نوبت سوم")]
        ThirdRound=3,
		[Display(Name = "نوبت چهارم")]
		FourthRound = 4,
	}
}
