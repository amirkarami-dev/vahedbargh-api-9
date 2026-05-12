using System.ComponentModel.DataAnnotations;

namespace Coreapi.Common.Enums
{
    public enum TransactionStatusEnum
    {
        [Display(Name = "واریز")]
        In,
        [Display(Name = "برداشت")]
        Out,
        None
        
    }
}
