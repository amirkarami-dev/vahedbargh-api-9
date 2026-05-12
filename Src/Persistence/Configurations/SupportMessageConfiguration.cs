using Coreapi.Domain.AggregatesModel.SupportAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Coreapi.Persistence.Configurations;

public class SupportMessageConfiguration : IEntityTypeConfiguration<SupportMessage>
{
    public void Configure(EntityTypeBuilder<SupportMessage> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.Property(e => e.UserId)
            .IsRequired();
        builder.Property(e => e.ToUserId)
            .IsRequired();

        builder.HasOne(p => p.Support)
            .WithMany()
            .HasForeignKey("SupportId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);




        builder.Property<DateTime>("Created");
        builder.Property<DateTime>("LastModified");
    }
}