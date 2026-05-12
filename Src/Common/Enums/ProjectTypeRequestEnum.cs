using System.ComponentModel.DataAnnotations;

namespace Coreapi.Common.Enums;

public enum ProjectTypeRequestEnum
{
    [Display(Name = "انشعاب دائم/موقت")]
    Pr0,
    [Display(Name = "فیبر نوری")]
    Pr1,
    [Display(Name = "تست و تحویل")]
    Pr2

}