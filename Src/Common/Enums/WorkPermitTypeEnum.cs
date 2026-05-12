using System.ComponentModel.DataAnnotations;

namespace Coreapi.Common.Enums;

public enum WorkPermitTypeEnum
{
    [Display(Name = "نامشخص")]
    None,
    [Display(Name = "بازرسی")]
    Inspection,
    [Display(Name = "ارت")]
    Earth,
    [Display(Name = "بازرسی و ارت")]
    InspectionAndEarth
}