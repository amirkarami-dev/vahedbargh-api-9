using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Coreapi.Application.Features.Transactions.Queries.GetClientInvoices
{
    public class GetClientInvoiceQuery:IRequest<IEnumerable<ClientInvoiceDto>>
    {

    }
}
