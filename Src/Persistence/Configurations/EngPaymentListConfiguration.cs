using Coreapi.Domain.AggregatesModel.FinanceAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Coreapi.Persistence.Configurations;

public class EngPaymentListConfiguration:IEntityTypeConfiguration<EngPaymentList>
{
    public void Configure(EntityTypeBuilder<EngPaymentList> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(p => p.Id).ValueGeneratedNever();
        builder.HasOne(p => p.EngPaymentTask)
            .WithMany()
            .HasForeignKey("EngPaymentTaskId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.Transaction)
            .WithOne(p => p.EngPaymentList)
            .HasForeignKey(typeof(EngPaymentList), "TransactionId")
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Engineer)
            .WithMany()
            .HasForeignKey("EngineerId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property<DateTime>("Created");
        builder.Property<DateTime>("LastModified");
    }
}