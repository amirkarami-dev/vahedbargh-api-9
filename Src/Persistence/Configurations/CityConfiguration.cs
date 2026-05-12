using Coreapi.Domain.AggregatesModel.GeoAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coreapi.Persistence.Configurations;

public class CityConfiguration:IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.HasOne(p => p.Province)
            .WithMany()
            .HasForeignKey("ProvinceId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}