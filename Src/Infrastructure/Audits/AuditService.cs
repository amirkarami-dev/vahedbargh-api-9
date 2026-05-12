using System.Collections.Generic;
using System.Threading.Tasks;
using Coreapi.Common.Models;
using Coreapi.Domain.SeedWork;

namespace Coreapi.Infrastructure.Audits;

public class AuditService(AuditDbContext auditContext):IAuditService
{
    public async Task AddAuditLogsAsync(List<ElecAuditLog> auditLogs)
    {
        await auditContext.ElecAuditLogs.AddRangeAsync(auditLogs);


        await auditContext.SaveChangesAsync();
    }

    public async Task AddSmsLogsAsync(ElecSmsLog smsLogs)
    {
        await auditContext.ElecSmsLog.AddAsync(smsLogs);
        await auditContext.SaveChangesAsync();

    }
}
