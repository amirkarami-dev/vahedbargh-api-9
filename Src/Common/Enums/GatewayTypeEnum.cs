using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Common.Enums
{
    public enum GatewayTypeEnum
    {
        [Display(Name = "کاربر")]
        Custom,
        [Display(Name = "درگاه ملی")]
        Melli,
        [Display(Name = "برگشتی")]
        BackPay,
        [Display(Name = "سیستمی")]
        System,
        [Display(Name = "ایرانکیش")]
        IranKish
    }
}
