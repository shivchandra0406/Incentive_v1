using Incentive.Core.Entities;
using Incentive.Core.Entities.IncentivePlan;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Incentive.Infrastructure.Data.EntityConfigurations
{
    public class ProjectBasedIncentivePlanConfiguration : IEntityTypeConfiguration<ProjectBasedIncentivePlan>
    {
        public void Configure(EntityTypeBuilder<ProjectBasedIncentivePlan> builder)
        {
            builder.Property(p => p.ProjectId)
                .IsRequired();

            builder.Property(p => p.MetricType)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(p => p.TargetValue)
                .IsRequired()
                .HasColumnType("decimal(18, 2)");

            builder.Property(p => p.IncentiveValue)
                .IsRequired()
                .HasColumnType("decimal(18, 2)");

            builder.Property(p => p.CalculationType)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(p => p.IncentiveAfterExceedingTarget)
                .IsRequired();

            // Configure relationships
            builder.HasOne(p => p.Project)
                .WithMany()
                .HasForeignKey(p => p.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
