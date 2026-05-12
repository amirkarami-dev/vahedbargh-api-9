using System.ComponentModel.DataAnnotations;

namespace Coreapi.Common.Enums
{
    public enum FieldTypeEnum
    {
        [Display(Name = "نامشخص")]
        None = 0,
        [Display(Name = "طراحی")]
        Design = 1,
        [Display(Name = "نظارت")]
        Supervision = 2,
        [Display(Name = "طراحی و نظارت")]
        DesignAndSupervision = 3,


    }
}
