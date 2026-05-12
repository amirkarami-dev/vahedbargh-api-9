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
    public class ElectProjectFileConfiguration:IEntityTypeConfiguration<ElectProjectFile>
    {
        public void Configure(EntityTypeBuilder<ElectProjectFile> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();

            builder.Property(b => b.FileTypeEnum).IsRequired();
            builder.Property(b => b.UserId).IsRequired();
            builder.Property(b => b.FolderName).IsRequired();
            builder.Property(b => b.FileName).IsRequired();

            builder.HasOne(p => p.ElectProject)
                .WithMany()
                .HasForeignKey("ElectProjectId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);



            builder.Property<DateTime>("Created");
            builder.Property<DateTime>("LastModified");
        }
    }
}
