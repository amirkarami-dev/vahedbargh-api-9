using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Domain.AggregatesModel.QuarterTariffAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coreapi.Persistence.Configurations
{
    public class QuarterTariffConfiguration:IEntityTypeConfiguration<QuarterTariff>
    {
        public void Configure(EntityTypeBuilder<QuarterTariff> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(b => b.Id).ValueGeneratedNever();
            builder.Property(b => b.Fee).IsRequired();
            builder.Property(b => b.Year).IsRequired();
            builder.Property(b => b.QuarterTypeEnum).IsRequired();
            builder.Property(b => b.AllotmentRoundTypeEnum).IsRequired();
            builder.Property(b => b.JulianAllotmentDate).IsRequired();
            builder.Property(b => b.SolarAllotmentDate).IsRequired();


            builder.Property<DateTime>("Created");
            builder.Property<DateTime>("LastModified");
        }
    }
}
