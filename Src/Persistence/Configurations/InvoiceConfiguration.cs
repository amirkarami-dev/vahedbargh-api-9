using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coreapi.Domain.AggregatesModel.FinanceAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coreapi.Persistence.Configurations
{
    public class InvoiceConfiguration:IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();

            builder.Property(b => b.Amount).IsRequired();


            builder.HasOne(p => p.Transaction)
                .WithOne(p => p.Invoice)
                .HasForeignKey(typeof(Invoice), "TransactionId")
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.ElectProject)
                .WithMany()
                .HasForeignKey("ElectProjectId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.ElectProjectProcess)
                .WithMany()
                .HasForeignKey("ElectProjectProcessId")
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Client)
                .WithMany()
                .HasForeignKey("ClientId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property<DateTime>("Created");
            builder.Property<DateTime>("LastModified");

        }
    }
}
