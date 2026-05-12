using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Common.Enums
{
    public enum UserFileTypeEnum
    {
        [Display(Name = "نامشخص")]
        F0,
        [Display(Name = "کارت ملی")]
        F1,
        [Display(Name = "مهر و امضاء")]
        F2,
        [Display(Name = "پروانه")]
        F3,
        [Display(Name = "مدرک تست و تحویل")]
        F4,
        [Display(Name = "مدرک ارت")]
        F5,
        [Display(Name = "مدرک فیبر نوری")]
        F6,
        [Display(Name = "گواهی بازرسی برق اماکن")]
        F7,
        [Display(Name = "عکس پرسنلی")]
        F8,
        [Display(Name = "نامه و درخواست")]
        F9
    }
}
