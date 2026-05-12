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
    internal class TransactionConfiguration:IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id)
                .ValueGeneratedNever();

            builder.Property(b => b.UserId)
                .IsRequired();
            builder.Property(b => b.GatewayType)
                .IsRequired();
            builder.Property(b => b.TransactionStatus)
                .IsRequired();
            builder.Property(b => b.TransactionType)
                .IsRequired();
            builder.Property(b => b.BankTransactionId)
                .IsRequired();
            builder.Property(b => b.Des)
                .IsRequired();

            builder.Property<DateTime>("Created");
            builder.Property<DateTime>("LastModified");
        }
    }
}
