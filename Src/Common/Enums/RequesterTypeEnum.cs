using System.ComponentModel.DataAnnotations;

namespace Coreapi.Common.Enums;

public enum RequesterTypeEnum
{
    [Display(Name = "شرکت توزیع")]
    ReqEdc,
    [Display(Name = "شرکت مخابرات")]
    ReqTci,
    [Display(Name = "مالک")]
    ReqOwner
}