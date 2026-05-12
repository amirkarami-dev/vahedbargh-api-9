using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Coreapi.Domain.AggregatesModel.ClientAggregate;

namespace Coreapi.Persistence.Configurations;

public class ClientUserCaptureConfiguration : IEntityTypeConfiguration<ClientUserCapture>
{
    public void Configure(EntityTypeBuilder<ClientUserCapture> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.Property(b => b.UserId)
                .HasMaxLength(40)
                .IsRequired();

        builder.Property(b => b.Latitude)
                .IsRequired();

        builder.Property(b => b.Longitude)
                .IsRequired();

        builder.Property(b => b.RefId)
                .IsRequired();

        builder.Property(b => b.RefType)
                .IsRequired();

        builder.Property(b => b.EventType)
                .IsRequired();

        builder.Property(b => b.Status)
                .IsRequired();

        builder.Property(b => b.Created)
                .IsRequired();

        builder.HasOne(p => p.Client)
          .WithMany()
          .HasForeignKey("ClientId")
          .IsRequired()
          .OnDelete(DeleteBehavior.Cascade);
    }
}
