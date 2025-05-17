using Incentive.Core.Entities.IncentivePlan;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Incentive.Infrastructure.Data.EntityConfigurations
{
    public class TargetBasedIncentivePlanConfiguration : IEntityTypeConfiguration<TargetBasedIncentivePlan>
    {
        public void Configure(EntityTypeBuilder<TargetBasedIncentivePlan> builder)
        {
            builder.Property(p => p.TargetType)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(p => p.Salary)
                .HasColumnType("decimal(18, 2)");

            builder.Property(p => p.MetricType)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(p => p.TargetValue)
                .IsRequired()
                .HasColumnType("decimal(18, 2)");

            builder.Property(p => p.CalculationType)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(p => p.IncentiveValue)
                .IsRequired()
                .HasColumnType("decimal(18, 2)");

            builder.Property(p => p.IncentiveAfterExceedingTarget)
                .IsRequired();

            builder.Property(p => p.ProvideAdditionalIncentiveOnExceeding);

            builder.Property(p => p.IncludeSalaryInTarget)
                .IsRequired();
        }
    }
}
