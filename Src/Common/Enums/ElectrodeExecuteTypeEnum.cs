using System.ComponentModel.DataAnnotations;

namespace Coreapi.Common.Enums;

// روش اجرای الکترود
public enum ElectrodeExecuteTypeEnum
{
    [Display(Name = "نامشخص")]
    None,
    [Display(Name = "قائم - کوبشی")]
    Eet1,
    [Display(Name = "قائم - عمیق")]
    Eet2,
    [Display(Name = "قائم - حفر چاه")]
    Eet3,
    [Display(Name = "افقی")]
    Eet4,
    [Display(Name = "سایر")]
    Other

}