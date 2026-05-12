using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Coreapi.Application.Features.Transactions.Commands.PaymentMelli
{
    public class PaymentMelliCommand:IRequest<string>
    {
        public int Amount { get; set; }
        public string ElectProjectId { get; set; }

    }
}
