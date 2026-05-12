using System.ComponentModel.DataAnnotations;

namespace Coreapi.Common.Enums
{
    public enum TransactionTypeEnum
    {
        [Display(Name = "کلاینت")]
        Client,
        [Display(Name = "شرکت")]
        Company
    }
}
