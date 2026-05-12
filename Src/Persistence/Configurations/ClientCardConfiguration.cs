using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Coreapi.Domain.AggregatesModel.ClientAggregate;

namespace Coreapi.Persistence.Configurations
{
    public class ClientCardConfiguration : IEntityTypeConfiguration<ClientCard>
    {
        public void Configure(EntityTypeBuilder<ClientCard> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();

            builder.Property(b => b.NameOnCard)
                    .HasMaxLength(50)
                    .IsRequired();

            builder.Property(b => b.CardNumber)
                    .HasMaxLength(32)
                    .IsRequired();

            builder.Property(b => b.CCV)
                    .HasMaxLength(10)
                    .IsRequired();

            builder.Property(b => b.ExpireDate)
                    .HasMaxLength(10)
                    .IsRequired();

            builder.Property(b => b.Type)
                    .IsRequired();

            builder.HasIndex(p => new { p.CardNumber, p.Type }).IsUnique();
        }
    }
}
