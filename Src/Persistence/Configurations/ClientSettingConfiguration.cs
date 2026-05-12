using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Coreapi.Domain.AggregatesModel.ClientAggregate;

namespace Coreapi.Persistence.Configurations
{
    public class ClientSettingConfiguration : IEntityTypeConfiguration<ClientSetting>
    {
        public void Configure(EntityTypeBuilder<ClientSetting> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();

            builder.Property(b => b.InMessage)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(b => b.OutMessage)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(b => b.BreakOutMessage)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(b => b.BreakInMessage)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(b => b.AllowedOvertime).IsRequired();
            builder.Property(b => b.AllowedBreak).IsRequired();
            builder.Property(b => b.AllowedWorkHour).IsRequired();
            builder.Property(b => b.AutoOutCycle).HasDefaultValue(4).IsRequired();
            builder.Property(b => b.DefaultFirstOffDay).IsRequired();
            builder.Property(b => b.DefaultSecondOffDay).IsRequired(false);
        }
    }
}
