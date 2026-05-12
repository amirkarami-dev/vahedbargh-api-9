using Coreapi.Domain.AggregatesModel.ErtTariffAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Coreapi.Persistence.Configurations;

public class ErtTariffConfiguration:IEntityTypeConfiguration<ErtTariff>
{
    public void Configure(EntityTypeBuilder<ErtTariff> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(b => b.UserId).IsRequired();
        builder.Property(b => b.ErtSystemTypeEnum).IsRequired();
        builder.Property(b => b.JulianDateExecute).IsRequired();
        builder.Property(b => b.SolarDateExecute).IsRequired();


        builder.Property<DateTime>("Created");
        builder.Property<DateTime>("LastModified");

    }
}