using Coreapi.Domain.AggregatesModel.RoutAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coreapi.Persistence.Configurations;

public class RoutConfiguration:IEntityTypeConfiguration<Route>
{
    public void Configure(EntityTypeBuilder<Route> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .ValueGeneratedNever();
        builder.Property(b => b.RoutName)
            .IsRequired();
        builder.Property(p => p.RoutAddress)
            .IsRequired();
        builder.Property(p => p.RoutAddressEn)
            .IsRequired();
        
        builder.HasOne(p=>p.Parent)
            .WithMany(c=>c.SubRoutes)
            .HasForeignKey(c=>c.ParentId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }
}