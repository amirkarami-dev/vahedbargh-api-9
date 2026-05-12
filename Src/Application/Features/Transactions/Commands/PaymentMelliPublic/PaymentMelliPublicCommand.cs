using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Coreapi.Application.Features.Transactions.Commands.PaymentMelliPublic
{
    public class PaymentMelliPublicCommand:IRequest<string>
    {
        public long Amount { get; set; }
        public string ElectProjectId { get; set; }
    }
}
