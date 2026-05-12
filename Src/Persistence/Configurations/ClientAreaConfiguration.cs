using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Coreapi.Domain.AggregatesModel.ClientAggregate;

namespace Coreapi.Persistence.Configurations;

public class ClientAreaConfiguration : IEntityTypeConfiguration<ClientArea>
{
    public void Configure(EntityTypeBuilder<ClientArea> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.Property(b => b.Name)
                .HasMaxLength(200)
                .IsRequired();

        builder.Property(b => b.Description)
                .HasMaxLength(500)
                .IsRequired();

        builder.Property(b => b.Type)
                .IsRequired();

        builder.Property(b => b.Radius)
                .IsRequired();

        builder.Property(b => b.Area)
                .IsRequired()
                .HasColumnType("geometry");
        
        builder.HasMany(p => p.Users)
          .WithOne()
          .HasForeignKey("ClientAreaId")
          .IsRequired()
          .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Points)
          .WithOne()
          .HasForeignKey("ClientAreaId")
          .IsRequired()
          .OnDelete(DeleteBehavior.Cascade);

        builder.Property<DateTime>("Created");
        builder.Property<DateTime>("LastModified");

    }
}
