using System.ComponentModel.DataAnnotations;

namespace Coreapi.Common.Enums;

public enum FilterCertEnum
{
    [Display(Name = "همه")]
    Fc0,
    [Display(Name = "بازرسی")]
    Fc1,
    [Display(Name = "ارت")]
    Fc2,
    [Display(Name = "تست و تحویل")]
    Fc3,

}