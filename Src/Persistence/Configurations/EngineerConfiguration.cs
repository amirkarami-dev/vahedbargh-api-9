using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Domain.AggregatesModel.EngineerAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coreapi.Persistence.Configurations
{
    public class EngineerConfiguration:IEntityTypeConfiguration<Engineer>
    {
        public void Configure(EntityTypeBuilder<Engineer> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(b => b.IdSection)
                .IsRequired();
            builder.Property(b => b.CellPhone)
                .IsRequired();
            builder.Property(b => b.NaCode)
                .IsRequired();
            builder.Property(b => b.UserId)
                .IsRequired();
            builder.Property(p => p.NaCode).HasMaxLength(10).IsRequired();
            builder.HasIndex(p => new { p.NaCode }).IsUnique();



            builder.Property<DateTime>("Created");
            builder.Property<DateTime>("LastModified");

        }
    }
}
