using Incentive.Core.Entities;
using Incentive.Core.Entities.IncentivePlan;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Incentive.Infrastructure.Data.EntityConfigurations
{
    public class RoleBasedIncentivePlanConfiguration : IEntityTypeConfiguration<RoleBasedIncentivePlan>
    {
        public void Configure(EntityTypeBuilder<RoleBasedIncentivePlan> builder)
        {
            builder.Property(p => p.Role)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.IsTeamBased)
                .IsRequired();

            builder.Property(p => p.TargetType)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(p => p.SalaryPercentage)
                .HasColumnType("decimal(5, 2)");

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

            builder.Property(p => p.IsCumulative)
                .IsRequired();

            builder.Property(p => p.IncentiveAfterExceedingTarget)
                .IsRequired();

            builder.Property(p => p.IncludeSalaryInTarget)
                .IsRequired();

            // Configure relationships if any
            builder.HasOne<Team>()
                .WithMany()
                .HasForeignKey(p => p.TeamId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
