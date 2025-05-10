using Incentive.Core.Entities;
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
                .HasMaxLength(100);

            builder.Property(r => r.Description)
                .HasMaxLength(500);

            builder.Property(r => r.Type)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(r => r.Value)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(r => r.MinBookingValue)
                .HasColumnType("decimal(18,2)");

            builder.Property(r => r.MaxBookingValue)
                .HasColumnType("decimal(18,2)");

            // New properties
            builder.Property(r => r.Frequency)
                .HasMaxLength(50)
                .HasDefaultValue("Monthly");

            builder.Property(r => r.AppliedTo)
                .HasMaxLength(50)
                .HasDefaultValue("User");

            builder.Property(r => r.CurrencyType)
                .HasMaxLength(10)
                .HasDefaultValue("USD");

            builder.Property(r => r.TargetType)
                .HasMaxLength(50)
                .HasDefaultValue("ItemBased");

            builder.Property(r => r.IncentiveType)
                .HasMaxLength(50)
                .HasDefaultValue("Percentage");

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
                .WithOne(i => i.IncentiveRule)
                .HasForeignKey(i => i.IncentiveRuleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
