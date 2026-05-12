using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Domain.AggregatesModel.BuildingTariffAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coreapi.Persistence.Configurations
{
    public class BuildingTariffConfiguration:IEntityTypeConfiguration<BuildingTariff>
    {
        public void Configure(EntityTypeBuilder<BuildingTariff> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(b => b.UserId).IsRequired();
            builder.Property(b => b.BuildingGroupTypeEnum).IsRequired();
            builder.Property(b => b.BuildingGroupParameterTypeEnum).IsRequired();
            builder.Property(b => b.JulianDateExecute).IsRequired();
            builder.Property(b => b.SolarDateExecute).IsRequired();


            builder.Property<DateTime>("Created");
            builder.Property<DateTime>("LastModified");

        }
    }
}
