using System.Collections.Generic;
using System.Threading.Tasks;
using Coreapi.Common.Models;

namespace Coreapi.Domain.SeedWork;

public interface IAuditService
{
    Task AddAuditLogsAsync(List<ElecAuditLog> auditLog);
    Task AddSmsLogsAsync(ElecSmsLog smsLogs);
}