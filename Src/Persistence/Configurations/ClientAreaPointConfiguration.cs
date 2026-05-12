using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Coreapi.Domain.AggregatesModel.ClientAggregate;

namespace Coreapi.Persistence.Configurations;

public class ClientAreaPointConfiguration : IEntityTypeConfiguration<ClientAreaPoint>
{
    public void Configure(EntityTypeBuilder<ClientAreaPoint> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(b => b.Order)
                .IsRequired();

        builder.OwnsOne(b => b.Location, point =>
        {
            point.Property(t => t.Latitude).HasColumnName("Latitude").IsRequired();

            point.Property(t => t.Longitude).HasColumnName("Longitude").IsRequired();
        });

    }
}
