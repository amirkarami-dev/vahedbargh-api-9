using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Domain.AggregatesModel.ExecutorAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coreapi.Persistence.Configurations
{
    internal class ExecutorConfigurations:IEntityTypeConfiguration<Executor>
    {
        public void Configure(EntityTypeBuilder<Executor> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();

            builder.HasIndex(e => e.NaCode).IsUnique();

            builder.Property(b => b.IdSection)
                .IsRequired();
            builder.Property(b => b.CellPhone)
                .IsRequired();
            builder.Property(p => p.NaCode).HasMaxLength(10).IsRequired();


            builder.Property<DateTime>("Created");
            builder.Property<DateTime>("LastModified");

        }
    }
}
