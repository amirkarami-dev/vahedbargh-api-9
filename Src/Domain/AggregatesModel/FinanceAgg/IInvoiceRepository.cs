using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Enums;
using Coreapi.Domain.SeedWork;

namespace Coreapi.Domain.AggregatesModel.FinanceAgg
{
    public interface IInvoiceRepository:IRepository<Invoice>
    {
        Task<Invoice> GetInvoiceByProjectId(Guid projectId, InvoicePayTypeEnum invoicePayType);
        Task<IEnumerable<Invoice>> GetInvoicesByProjectId(Guid projectId);
		Task<Invoice> GetInvoiceByProjectIdWithProcess(Guid projectId, InvoicePayTypeEnum invoicePayType);
        Task DeleteByIds(IEnumerable<Guid> ids);
    }
}
