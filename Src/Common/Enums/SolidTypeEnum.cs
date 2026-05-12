using System.ComponentModel.DataAnnotations;

namespace Coreapi.Common.Enums;

public enum SolidTypeEnum
{
    [Display(Name = "نام مشخص")]
    None,
    [Display(Name = "رسی")]
    Sandy,
    [Display(Name = "سنگلاخی")]
    Silty,
    [Display(Name = "شنی")]
    Clayey,
    [Display(Name = "سنگی")]
    Rocky
}