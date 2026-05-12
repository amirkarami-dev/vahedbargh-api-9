using Coreapi.Domain.AggregatesModel.FinanceAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Coreapi.Persistence.Configurations;

public class EngQuotaBurnConfiguration:IEntityTypeConfiguration<EngQuotaBurn>
{
    public void Configure(EntityTypeBuilder<EngQuotaBurn> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(p => p.Id).ValueGeneratedNever();

        builder.HasOne(p => p.Client)
            .WithMany()
            .HasForeignKey("ClientId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);


        builder.HasOne(p => p.QuarterTariff)
            .WithMany()
            .HasForeignKey("QuarterTariffId")
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.Engineer)
            .WithMany()
            .HasForeignKey("EngineerId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);


        builder.Property<DateTime>("Created");
        builder.Property<DateTime>("LastModified");
    }
}