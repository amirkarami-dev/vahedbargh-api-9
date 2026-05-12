using Coreapi.Common.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Coreapi.Infrastructure.Configurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<ElecAuditLog>
{
    public void Configure(EntityTypeBuilder<ElecAuditLog> builder)
    {
        builder.HasKey(e => e.Id);


        builder.OwnsMany(a => a.Changes, cb =>
        {
            cb.ToJson(); // EF 7 method to store as JSON
            cb.WithOwner().HasForeignKey("AuditLogId");
        });
    }
}