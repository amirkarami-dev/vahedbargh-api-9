using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Features.Clients.Commands.SignupClient
{
    public class SignupOutput
    {
        public Guid InvoiceId { get; set; }
        public decimal Amount { get; set; }
    }
}
