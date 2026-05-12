using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coreapi.Persistence.Configurations
{
    public class ElectProjectProcessConfiguration:IEntityTypeConfiguration<ElectProjectProcess>
    {
        public void Configure(EntityTypeBuilder<ElectProjectProcess> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();


            builder.HasOne(b => b.Client)
                .WithMany()
                .HasForeignKey("ClientId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Engineer)
                .WithMany()
                .HasForeignKey("EngineerId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.ElectProject)
                .WithMany()
                .HasForeignKey("ElectProjectId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.QuarterTariff)
                .WithMany()
                .HasForeignKey("QuarterTariffId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.BuildingTariff)
                .WithMany()
                .HasForeignKey("BuildingTariffId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);


            builder.Property(b => b.UserId).IsRequired();
            builder.Property(b => b.ProjectLevelEnum).IsRequired();
            builder.Property(b => b.InspectionStatusEnum).IsRequired();
            builder.Property(b => b.Defect).IsRequired();
            builder.Property(b => b.Fee).IsRequired();
            builder.Property(b => b.JulianRegisterDate).IsRequired();
            builder.Property(b => b.SolarRegisterDate).IsRequired();


            builder.Property<DateTime>("Created");
            builder.Property<DateTime>("LastModified");

        }
    }
}
