using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Common.Models.Bank
{
    public class RequestPaymentResultMelliModel
    {
        public int ResCode { get; set; }
        public string Description { get; set; }
        public string Token { get; set; }
    }

    public class CallbackRequestPaymentMelliModel
    {
        public string PrimaryAccNo { get; set; }
        public string HashedCardNo { get; set; }
        public long OrderId { get; set; }
        public string SwitchResCode { get; set; }
        public int ResCode { get; set; }
        public string Token { get; set; }
    }

    public class VerifyResponsePaymentMelliModel
    {
        public int ResCode { get; set; }
        public int? Amount { get; set; }
        public string Description { get; set; }
        public string RetrivalRefNo { get; set; }
        public string SystemTraceNo { get; set; }
        public long? OrderId { get; set; }
    }
}
