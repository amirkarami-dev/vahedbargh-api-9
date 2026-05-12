using System.ComponentModel.DataAnnotations;

namespace Coreapi.Common.Enums
{
    public enum ExecutorGradTypeEnum
    {
        [Display(Name = "خانگی")]
        Household = 1,
        [Display(Name = "صنعتی")]
        Industrial = 2,
    }
}
