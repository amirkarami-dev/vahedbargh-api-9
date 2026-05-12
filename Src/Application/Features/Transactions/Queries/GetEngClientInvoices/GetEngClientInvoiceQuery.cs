using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Coreapi.Application.Features.Transactions.Queries.GetEngClientInvoices
{
    public class GetEngClientInvoiceQuery:IRequest<IEnumerable<EngClientInvoiceDto>>
    {
        public Guid EngId { get; set; }
    }
}
