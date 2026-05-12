using System.ComponentModel.DataAnnotations;

namespace Coreapi.Common.Enums
{
    public enum ExecutorTypeEnum
    {
        [Display(Name = "حرفه ای")]
        Professional = 1,
        [Display(Name = "تجربی")]
        Experiential = 2,
    }
}
