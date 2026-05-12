using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Coreapi.Persistence.Configurations;

public class ElectProjectConfiguration:IEntityTypeConfiguration<ElectProject>
{
    public void Configure(EntityTypeBuilder<ElectProject> builder)
    {

        builder.HasKey(e => e.Id);


        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.HasIndex(c => c.FileNumber).IsUnique();

        builder.HasOne(e => e.ParentProject)
            .WithMany(e => e.ChildProjects)
            .HasForeignKey("ParentProjectId")
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Client)
            .WithMany()
            .HasForeignKey("ClientId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.BuildingTariff)
            .WithMany()
            .HasForeignKey("BuildingTariffId")
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.ErtTariff)
            .WithMany()
            .HasForeignKey("ErtTariffId")
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.PanelMaker)
            .WithMany()
            .HasForeignKey("PanelMakerId")
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Section)
            .WithMany()
            .HasForeignKey("IdSection")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(b => b.Address)
            .IsRequired();
        builder.Property(b => b.LandlordNaCode)
            .IsRequired();
        builder.Property(b => b.LandlordPhoneNumber)
            .IsRequired();
        builder.Property(b => b.IdSection)
            .IsRequired();

        builder.Property(b => b.RequesterTypeEnum)
            .IsRequired();
        builder.Property(b => b.ProjectLevelEnum)
            .IsRequired();
        builder.Property(b => b.JulianRegisterDate)
            .IsRequired();
        builder.Property(b => b.SolarRegisterDate)
            .IsRequired();



        builder.Property<DateTime>("Created");
        builder.Property<DateTime>("LastModified");
    }
}