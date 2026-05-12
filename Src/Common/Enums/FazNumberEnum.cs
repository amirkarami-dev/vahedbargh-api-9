using System.ComponentModel.DataAnnotations;

namespace Coreapi.Common.Enums;

public enum FazNumberEnum
{
    [Display(Name = "تک فاز")]
    OneFaz,
    [Display(Name = "سه فاز")]
    TreeFaz,
    [Display(Name = "انشعاب دائم")]
    PermanentBranch,
    [Display(Name = "انشعاب موقت")]
    TemporaryBranch
}