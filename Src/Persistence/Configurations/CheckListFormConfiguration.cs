using System;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coreapi.Persistence.Configurations;

public class CheckListFormConfiguration:IEntityTypeConfiguration<CheckListForm>
{
    public void Configure(EntityTypeBuilder<CheckListForm> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(b => b.Id).ValueGeneratedNever();

        builder.HasOne(p => p.Engineer)
            .WithMany()
            .HasForeignKey("EngineerId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);


        builder.HasOne(p => p.ElectProject)
            .WithMany()
            .HasForeignKey("ElectProjectId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property<DateTime>("Created");
        builder.Property<DateTime>("LastModified");

    }
}