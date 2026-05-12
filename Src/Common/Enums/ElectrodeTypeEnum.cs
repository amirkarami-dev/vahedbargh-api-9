using System.ComponentModel.DataAnnotations;

namespace Coreapi.Common.Enums;

// نوع الکترود
public enum ElectrodeTypeEnum
{
    [Display(Name = "نامشخص")]
    None,
    [Display(Name = "میله")]
    Et1,
    [Display(Name = "لوله")]
    Et2,
    [Display(Name = "تسمه")]
    Et3,
    [Display(Name = "سیم")]
    Et4,
    [Display(Name = "صفحه")]
    Et5,
    [Display(Name = "سایر")]
    Other
}