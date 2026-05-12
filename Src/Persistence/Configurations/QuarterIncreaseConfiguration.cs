using Coreapi.Domain.AggregatesModel.QuarterIncrease;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Coreapi.Persistence.Configurations;

public class QuarterIncreaseConfiguration:IEntityTypeConfiguration<QuarterIncrease>
{
    public void Configure(EntityTypeBuilder<QuarterIncrease> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(p => p.Id).ValueGeneratedNever();

        builder.HasOne(p => p.Engineer)
            .WithMany()
            .HasForeignKey("EngineerId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.QuarterTariff)
            .WithMany()
            .HasForeignKey("QuarterTariffId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);



        builder.Property<DateTime>("Created");
        builder.Property<DateTime>("LastModified");
    }
}