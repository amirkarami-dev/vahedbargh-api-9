using Coreapi.Domain.AggregatesModel.SupportAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Coreapi.Persistence.Configurations;

public class SupportFileConfiguration:IEntityTypeConfiguration<SupportFile>
{
    public void Configure(EntityTypeBuilder<SupportFile> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.Property(b => b.FileTypeEnum).IsRequired();
        builder.Property(b => b.UserId).IsRequired();
        builder.Property(b => b.FolderName).IsRequired();
        builder.Property(b => b.FileName).IsRequired();

        builder.HasOne(p => p.Support)
            .WithMany()
            .HasForeignKey("SupportId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property<DateTime>("Created");
        builder.Property<DateTime>("LastModified");
    }
}