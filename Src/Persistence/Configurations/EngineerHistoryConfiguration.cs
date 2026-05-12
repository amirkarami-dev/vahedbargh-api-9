using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Coreapi.Persistence.Configurations
{
    public class EngineerHistoryConfiguration : IEntityTypeConfiguration<Domain.AggregatesModel.EngineerAgg.EngineerHistory>
    {
        public void Configure(EntityTypeBuilder<Domain.AggregatesModel.EngineerAgg.EngineerHistory> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();



            builder.Property(b => b.WorkPermitNum)
                .IsRequired();
            builder.Property(b => b.JulianIssueDate)
                .IsRequired();
            builder.Property(b => b.SolarIssueDate)
                .IsRequired();
            builder.Property(b => b.WorkPermission)
                .IsRequired();
            builder.Property(b => b.EngineerGradeTypeEnum)
                .IsRequired();

            builder.Property<DateTime>("Created");
            builder.Property<DateTime>("LastModified");
        }
    }
}
