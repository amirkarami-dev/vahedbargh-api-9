using System.ComponentModel.DataAnnotations;

namespace Coreapi.Common.Enums
{
    public enum InvoiceStatusEnum
    {
        [Display(Name = "انتظار پرداخت")]
        Pending,
        [Display(Name = "کنسل شده")]
        Cancelled,
        [Display(Name = "پرداخت شده")]
        Done
    }
}
