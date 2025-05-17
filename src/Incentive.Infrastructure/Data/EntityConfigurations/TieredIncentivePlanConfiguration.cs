using Incentive.Core.Entities.IncentivePlan;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Incentive.Infrastructure.Data.EntityConfigurations
{
    public class TieredIncentivePlanConfiguration : IEntityTypeConfiguration<TieredIncentivePlan>
    {
        public void Configure(EntityTypeBuilder<TieredIncentivePlan> builder)
        {
            builder.Property(p => p.MetricType)
                .IsRequired()
                .HasConversion<string>();

            // Configure relationships
            builder.HasMany(p => p.Tiers)
                .WithOne(t => t.TieredIncentivePlan)
                .HasForeignKey(t => t.TieredIncentivePlanId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
