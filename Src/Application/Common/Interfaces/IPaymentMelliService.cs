using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Application.Common.Models.Bank;

namespace Coreapi.Application.Common.Interfaces
{
    public interface IPaymentMelliService
    {
        Task<TokenResult> GetPaymentMelliUrl(long amount, string paymentId);
        Task<TokenResult> GetPaymentMelliPublicUrl(long amount, string paymentId);
        Task<VerifyResult> VerifyPaymentMelli(string retrievalReferenceNumber, string systemTraceAuditNumber, string token);

        string GetVerifyCallBackUrl();
        string GetVerifyCallBackPublicUrl();
    }
}
