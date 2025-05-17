using Incentive.Core.Entities.IncentivePlan;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Incentive.Infrastructure.Data.EntityConfigurations
{
    public class IncentivePlanBaseConfiguration : IEntityTypeConfiguration<IncentivePlanBase>
    {
        public void Configure(EntityTypeBuilder<IncentivePlanBase> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.PlanName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.PlanType)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(p => p.PeriodType)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(p => p.IsActive)
                .IsRequired();

            builder.Property(p => p.CurrencyType)
                .IsRequired()
                .HasConversion<string>();

            // Configure TPH inheritance
            builder.HasDiscriminator(p => p.PlanDiscriminator)
                .HasValue<TargetBasedIncentivePlan>("TargetBased")
                .HasValue<RoleBasedIncentivePlan>("RoleBased")
                .HasValue<ProjectBasedIncentivePlan>("ProjectBased")
                .HasValue<KickerIncentivePlan>("KickerBased")
                .HasValue<TieredIncentivePlan>("TieredBased");
        }
    }
}
