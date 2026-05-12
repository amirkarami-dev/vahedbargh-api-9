using System.ComponentModel.DataAnnotations;

namespace Coreapi.Common.Enums;

public enum FileApprovedTypeEnum
{

    [Display(Name = "فرم تایید شمار3")]
    IdCard = 1,
    [Display(Name = "فرم تایید چک لیست")]
    ElectPlan = 2,
    [Display(Name = "فرم تایید مجری ارت")]
    RelatedPermit = 3,
    [Display(Name = "فرم تایید نظام مهندسی")]
    ErtMap = 4
}