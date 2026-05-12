using Coreapi.Domain.AggregatesModel.GeoAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coreapi.Persistence.Configurations;

public class SectionConfiguration:IEntityTypeConfiguration<Section>
{
    public void Configure(EntityTypeBuilder<Section> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.HasOne(p => p.City)
            .WithMany()
            .HasForeignKey("CityId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}