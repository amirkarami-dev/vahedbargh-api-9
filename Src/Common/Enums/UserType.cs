using System.ComponentModel.DataAnnotations;

namespace Coreapi.Common.Enums;

public enum UserType
{
    [Display(Name = "نامشخص")]
    None,
    [Display(Name = "کارشناس")]
    Engineer,
    [Display(Name = "مجری")]
    Executor
}