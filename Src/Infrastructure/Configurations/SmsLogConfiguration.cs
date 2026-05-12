using Coreapi.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coreapi.Infrastructure.Configurations;

public class SmsLogConfiguration:IEntityTypeConfiguration<ElecSmsLog>
{
    public void Configure(EntityTypeBuilder<ElecSmsLog> builder)
    {
        builder.HasKey(e => e.Id);

    }
}