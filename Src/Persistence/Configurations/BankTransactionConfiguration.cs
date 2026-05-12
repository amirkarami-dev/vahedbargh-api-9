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
    public class BankTransactionConfiguration:IEntityTypeConfiguration<BankTransaction>
    {
        public void Configure(EntityTypeBuilder<BankTransaction> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();


            builder.HasOne(p => p.Client)
                .WithMany()
                .HasForeignKey("ClientId")
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
            builder.Property(b => b.UserId)
                .IsRequired();
            builder.Property(b => b.PaymentId)
                .IsRequired();
            builder.Property(b => b.Amount)
                .IsRequired();
            builder.Property(b => b.PaymentTypeEnum)
                .IsRequired();
            builder.Property(b => b.GatewayTypeEnum)
                .IsRequired();
            builder.Property(b => b.Token)
                .IsRequired();



            builder.Property<DateTime>("Created");
            builder.Property<DateTime>("LastModified");


        }
    }
}
