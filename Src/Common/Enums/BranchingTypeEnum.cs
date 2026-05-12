using System.ComponentModel.DataAnnotations;

namespace Coreapi.Common.Enums;

public enum BranchingTypeEnum
{
    [Display(Name = "خانگی")]
    Bt1 = 0,
    [Display(Name = "عمومی")]
    Bt2 = 1,
    [Display(Name = "صنعتی")]
    Bt3 = 2,
    [Display(Name = "سایر مصارف")]
    Bt4 = 3,
    [Display(Name = "انشعاب موجود در محل")]
    Bt5 = 4

}