using Coreapi.Domain.AggregatesModel.FinanceAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Coreapi.Persistence.Configurations;

public class EngPaymentTaskConfiguration:IEntityTypeConfiguration<EngPaymentTask>
{
    public void Configure(EntityTypeBuilder<EngPaymentTask> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(p => p.Id).ValueGeneratedNever();

        builder.HasOne(p => p.Client)
            .WithMany()
            .HasForeignKey("ClientId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);


        builder.Property<DateTime>("Created");
        builder.Property<DateTime>("LastModified");

    }
}