using System.ComponentModel.DataAnnotations;

namespace Coreapi.Common.Enums;

public enum SmsTypeEnum
{

    [Display(Name = "هزینه پرونده")]
    Payment,
    [Display(Name = "بازگذاری مدارک")]
    Upload 
}