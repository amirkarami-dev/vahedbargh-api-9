using System.ComponentModel.DataAnnotations;

namespace Coreapi.Common.Enums
{
    public enum WidgetStatusEnum
    {
        [Display(Name = "Active")]
        Active = 1,
        [Display(Name = "Deleted")]
        Inactive = 2,
    }
}
