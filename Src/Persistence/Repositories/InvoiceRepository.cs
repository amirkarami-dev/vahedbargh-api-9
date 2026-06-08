using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Common.Enums;
using Coreapi.Domain.AggregatesModel.FinanceAgg;
using Microsoft.EntityFrameworkCore;


namespace Coreapi.Persistence.Repositories
{
    public class InvoiceRepository(CoreapiDbContext context) : BaseRepository<Invoice>(context), IInvoiceRepository
    {
        public async Task<Invoice> GetInvoiceByProjectId(Guid projectId, InvoicePayTypeEnum invoicePayType)
        {
            return await context.Invoices
                .Include(i=>i.ElectProjectProcess)
                .Include(i=>i.ElectProject)
                .Include(t=>t.Transaction)
                .FirstOrDefaultAsync(c => c.ElectProject.Id == projectId 
                                          && c.InvoicePayType == invoicePayType 
                                          && c.ElectProjectProcess == null);
        }

		public async Task<IEnumerable<Invoice>> GetInvoicesByProjectId(Guid projectId)
		{
			return await context.Invoices
				.Include(i => i.ElectProjectProcess)
				.Include(i => i.ElectProject)
				.Include(t => t.Transaction)
				.Where(c => c.ElectProject.Id == projectId
										  && c.ElectProjectProcess == null).ToListAsync();
		}

		public async Task<Invoice> GetInvoiceByProjectIdWithProcess(Guid projectId, InvoicePayTypeEnum invoicePayType)
        {
            return await context.Invoices
                .Include(i => i.ElectProjectProcess)
                .Include(i => i.ElectProject)
                .Include(t => t.Transaction)
                .FirstOrDefaultAsync(c => c.ElectProject.Id == projectId
                                          && c.InvoicePayType == invoicePayType
                                          && c.ElectProjectProcess != null);
        }

        public async Task DeleteByIds(IEnumerable<Guid> ids)
        {
            var idList = ids.ToList();
            if (idList.Count == 0) return;

            var entities = await context.Invoices
                .Where(i => idList.Contains(i.Id))
                .ToListAsync();

            context.Invoices.RemoveRange(entities);
            await context.SaveChangesAsync();
        }
    }
}
