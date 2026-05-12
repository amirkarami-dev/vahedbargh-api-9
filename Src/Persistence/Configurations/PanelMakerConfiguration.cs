using Coreapi.Domain.AggregatesModel.PanelMakerAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Coreapi.Persistence.Configurations;

public class PanelMakerConfiguration:IEntityTypeConfiguration<PanelMaker>
{
    public void Configure(EntityTypeBuilder<PanelMaker> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(b => b.MobileNumber).IsRequired();

        builder.Property<DateTime>("Created");
        builder.Property<DateTime>("LastModified");

    }
}