using Incentive.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Incentive.Infrastructure.Persistence.Configurations
{
    public class IncentiveRuleConfiguration : IEntityTypeConfiguration<IncentiveRule>
    {
        public void Configure(EntityTypeBuilder<IncentiveRule> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(r => r.Description)
                .HasMaxLength(500);

            builder.Property(r => r.Type)
                .IsRequired();

            builder.Property(r => r.Value)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(r => r.MinBookingValue)
                .HasColumnType("decimal(18,2)");

            builder.Property(r => r.MaxBookingValue)
                .HasColumnType("decimal(18,2)");

            builder.Property(r => r.StartDate)
                .IsRequired();

            builder.Property(r => r.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Configure relationships
            builder.HasOne(r => r.Project)
                .WithMany(p => p.IncentiveRules)
                .HasForeignKey(r => r.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(r => r.IncentiveEarnings)
                .WithOne(e => e.IncentiveRule)
                .HasForeignKey(e => e.IncentiveRuleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
