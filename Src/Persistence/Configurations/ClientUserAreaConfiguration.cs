using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Coreapi.Domain.AggregatesModel.ClientAggregate;

namespace Coreapi.Persistence.Configurations;

public class ClientUserAreaConfiguration : IEntityTypeConfiguration<ClientUserArea>
{
    public void Configure(EntityTypeBuilder<ClientUserArea> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.Property(b => b.UserId)
                .HasMaxLength(40)
                .IsRequired();
    }
}
