using System.ComponentModel.DataAnnotations;

namespace Coreapi.Common.Enums;

// جنس الکترود
public enum ElectrodeMaterialTypeEnum
{
    [Display(Name = "نامشخص")]
    None,
    [Display(Name = "مس")]
    Emt1,
    [Display(Name = "فولاد گالوانیزه")]
    Emt2,
    [Display(Name = "فولاد زد زنگ")]
    Emt3,
    [Display(Name = "سایر")]
    Other
}