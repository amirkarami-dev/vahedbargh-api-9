using System.ComponentModel.DataAnnotations;

namespace Coreapi.Common.Enums;

public enum ProjectProcessTypeEnum
{
    [Display(Name = "تخصیص به کارشناس")]
    EngProcess,
    [Display(Name = "تخصیص به مجری ارت")]
    ErtProcess,
    [Display(Name = "تخصیص به تابلوساز")]
    ElectPlanProcess
}