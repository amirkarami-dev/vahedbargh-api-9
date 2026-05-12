using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Coreapi.Application.Features.Transactions.Commands.PaymentMelliReturn
{
    public class PaymentMelliReturnCommand:IRequest<string>
    {
        public string Token { get; set; }
        public string AcceptorId { get; set; }
        public string ResponseCode { get; set; }
        public string PaymentId { get; set; }
        public string RequestId { get; set; }
        public string Sha256OfPan { get; set; }
        public string RetrievalReferenceNumber { get; set; }
        public string Amount { get; set; }
        public string MaskedPan { get; set; }
        public string SystemTraceAuditNumber { get; set; }


    }
}
