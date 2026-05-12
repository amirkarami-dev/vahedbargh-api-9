using System.ComponentModel.DataAnnotations;

namespace Coreapi.Common.Enums
{
    public enum SectionStatusEnum
    {
        [Display(Name = "Planned")]
        Planned,
        [Display(Name = "Open")]
        Open,
        [Display(Name = "Todo")]
        Todo,
        [Display(Name = "Unfinished")]
        Unfinished,
        [Display(Name = "Doing")]
        Doing,
        [Display(Name = "Testing")]
        Testing,
        [Display(Name = "Reviewing")]
        Reviewing,
        [Display(Name = "Done")]
        Done,
        [Display(Name = "Closed")]
        Closed
    }
}
