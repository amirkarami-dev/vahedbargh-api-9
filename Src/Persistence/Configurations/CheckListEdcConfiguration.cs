using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Coreapi.Persistence.Configurations;

public class CheckListEdcConfiguration:IEntityTypeConfiguration<CheckListEdc>
{
    public void Configure(EntityTypeBuilder<CheckListEdc> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(b => b.Id).ValueGeneratedNever();


        builder.HasOne(p => p.ElectProject)
            .WithMany()
            .HasForeignKey("ElectProjectId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property<DateTime>("Created");
        builder.Property<DateTime>("LastModified");
    }
}