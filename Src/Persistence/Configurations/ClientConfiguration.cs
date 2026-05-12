using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Coreapi.Common.Utility;
using Coreapi.Domain.AggregatesModel.ClientAggregate;

namespace Coreapi.Persistence.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();

            builder.Property(b => b.Name)
                    .HasMaxLength(200)
                    .IsRequired();

            builder.Property(b => b.Subdomain)
                    .HasMaxLength(200)
                    .IsRequired();

            builder.Property(b => b.ABN)
                    .HasMaxLength(20)
                    .IsRequired();

            builder.Property(b => b.Balance)
                    .HasColumnType("money")
                    .IsRequired();

            builder.Property(b => b.ApiKey)
                    .HasMaxLength(100)
                    .HasDefaultValue(Helper.GenerateApiKey())
                    .IsRequired();

            builder.Property(b => b.ChatUrl)
                    .HasMaxLength(500);

            builder.Property(b => b.RocketChatToken)
                    .HasColumnType("nvarchar(MAX)");

            builder.Property(b => b.RocketChatId)
                    .HasMaxLength(500);


            builder.HasOne(p => p.Setting)
              .WithOne()
              .HasForeignKey(typeof(ClientSetting), "ClientId")
              .IsRequired()
              .OnDelete(DeleteBehavior.Cascade);


            builder.HasMany(p => p.Cards)
              .WithOne()
              .HasForeignKey("ClientId")
              .IsRequired()
              .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Analyzers)
              .WithOne()
              .HasForeignKey("ClientId")
              .IsRequired()
              .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.UserTrainings)
              .WithOne()
              .HasForeignKey("ClientId")
              .IsRequired()
              .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Areas)
              .WithOne()
              .HasForeignKey("ClientId")
              .IsRequired()
              .OnDelete(DeleteBehavior.Cascade);

            builder.Property<DateTime>("Created");
            builder.Property<DateTime>("LastModified");
        }
    }
}
