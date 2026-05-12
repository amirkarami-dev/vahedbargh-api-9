using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Coreapi.Domain.AggregatesModel.ClientAggregate;

namespace Coreapi.Persistence.Configurations
{
    public class ClientUserTrainingConfiguration : IEntityTypeConfiguration<ClientUserTraining>
    {
        public void Configure(EntityTypeBuilder<ClientUserTraining> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();

            builder.Property(b => b.UserId)
                    .HasMaxLength(40)
                    .IsRequired();

            builder.Property(b => b.Title)
                    .HasMaxLength(100)
                    .IsRequired();

            builder.Property(b => b.Link)
                    .IsRequired();
        }
    }
}
