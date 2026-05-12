using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Coreapi.Application.Features.Transactions.Queries.GetClientEngInvoiceReport
{
    public class EngInvoiceReportQuery:IRequest<IEnumerable<EngInvoiceReportDto>>
    {
        public Guid EngId { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set;} = DateTime.Now;
    }
}
