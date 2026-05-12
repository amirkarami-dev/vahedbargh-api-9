using Coreapi.Domain.AggregatesModel.SupportAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Coreapi.Persistence.Configurations;

public class SupportConfiguration : IEntityTypeConfiguration<Support>
{
    public void Configure(EntityTypeBuilder<Support> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.HasOne(p => p.Client)
            .WithMany()
            .HasForeignKey("ClientId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(b => b.Title)
            .IsRequired();
        builder.Property(b => b.SolarCreate)
            .IsRequired();
        builder.Property(b => b.JulianCreate)
            .IsRequired();
        builder.Property(b => b.UserId)
            .IsRequired();
        builder.Property(b => b.ToUserId)
            .IsRequired();
        builder.Property(b => b.JulianEndSupport)
            .IsRequired(false);


        builder.Property<DateTime>("Created");
        builder.Property<DateTime>("LastModified");
    }
}