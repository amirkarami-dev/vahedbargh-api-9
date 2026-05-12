using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Coreapi.Domain.AggregatesModel.ClientAggregate;

namespace Coreapi.Persistence.Configurations
{
    public class ClientUserSettingConfiguration : IEntityTypeConfiguration<ClientUserSetting>
    {
        public void Configure(EntityTypeBuilder<ClientUserSetting> builder)
        {
            builder.HasKey(e => e.UserId);
            builder.Property(e => e.UserId).ValueGeneratedNever();

            builder.Property(b => b.FirstOffDay).IsRequired();
            builder.Property(b => b.SecondOffDay).IsRequired(false);

        }
    }
}
