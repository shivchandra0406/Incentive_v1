using Incentive.Core.Entities;
using Incentive.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Incentive.Infrastructure.Data.EntityConfigurations
{
    public class IncentiveRuleConfiguration : IEntityTypeConfiguration<IncentiveRule>
    {
        public void Configure(EntityTypeBuilder<IncentiveRule> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(r => r.Description)
                .HasMaxLength(500);

            builder.Property(r => r.Frequency)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(r => r.AppliedTo)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(r => r.Currency)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(r => r.Target)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(r => r.Incentive)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(r => r.TargetValue)
                .HasColumnType("decimal(18,2)");

            builder.Property(r => r.Commission)
                .HasColumnType("decimal(18,2)");

            builder.Property(r => r.MinimumSalesThreshold)
                .HasColumnType("decimal(18,2)");

            builder.Property(r => r.MaximumIncentiveAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(r => r.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(r => r.IsIncludeSalary)
                .IsRequired()
                .HasDefaultValue(true);

            // No Project relationship
        }
    }
}
