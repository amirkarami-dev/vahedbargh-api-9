using Coreapi.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Coreapi.Infrastructure.Audits;

public class AuditDbContext(DbContextOptions<AuditDbContext> options) : DbContext(options)
{
    // Your audit-related tables
    public DbSet<ElecAuditLog> ElecAuditLogs { get; set; }

    public DbSet<ElecSmsLog> ElecSmsLog { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuditDbContext).Assembly);
    }
}
