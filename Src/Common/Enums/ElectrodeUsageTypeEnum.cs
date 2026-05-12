using System.ComponentModel.DataAnnotations;

namespace Coreapi.Common.Enums;

// نوع کاربری الکترود
public enum ElectrodeUsageTypeEnum
{
    [Display(Name = "نامشخص")]
    None = 0,
    [Display(Name = "الکترود زمین ایمنی")]
    Eut1,
    [Display(Name = "الکترود زمین صاعفه گیر")]
    Eut2,
    [Display(Name = "الکترود زمین عملکردی")]
    Eut3,
    [Display(Name = "سایر")]
    Other,
    [Display(Name = "الکترود زمین حفاظتی")]
    Eut4,
}