using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Common.Models
{
    public class SecurePaymentResponseModel
    {
        public DateTime CreatedAt { get; set; }
        public string MerchantCode { get; set; }
        public string CustomerCode { get; set; }
        public string Token { get; set; }
        public string Ip { get; set; }
        public string Amount { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public string OrderId { get; set; }
        public string BankTransactionId { get; set; }
        public string GatewayResponseCode { get; set; }
        public string GatewayResponseMessage { get; set; }
        public string ErrorCode { get; set; }
    }
}
