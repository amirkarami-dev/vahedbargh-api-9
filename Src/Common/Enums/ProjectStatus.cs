using System.ComponentModel.DataAnnotations;

namespace Coreapi.Common.Enums
{
    public enum ProjectStatus
    {
        [Display(Name = "Ongoing")]
        Ongoing = 1,
        [Display(Name = "Done")]
        Done = 2,
        [Display(Name = "Suspend")]
        Suspend = 3,
        [Display(Name = "Initial")]
        Initial = 4
    }
}
