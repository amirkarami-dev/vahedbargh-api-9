using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Common.Models
{
    public class StripePaymentResponseModel
    {
        public string Id { get; set; }
        public string InvoiceId { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public bool Status { get; set; }
    }
}
