using Incentive.Core.Entities.IncentivePlan;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Incentive.Infrastructure.Data.EntityConfigurations
{
    public class KickerIncentivePlanConfiguration : IEntityTypeConfiguration<KickerIncentivePlan>
    {
        public void Configure(EntityTypeBuilder<KickerIncentivePlan> builder)
        {
            builder.Property(p => p.Location)
                .HasMaxLength(200);

            builder.Property(p => p.MetricType)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(p => p.TargetValue)
                .IsRequired()
                .HasColumnType("decimal(18, 2)");

            builder.Property(p => p.ConsistencyMonths)
                .IsRequired();

            builder.Property(p => p.AwardType)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(p => p.AwardValue)
                .HasColumnType("decimal(18, 2)");
        }
    }
}
