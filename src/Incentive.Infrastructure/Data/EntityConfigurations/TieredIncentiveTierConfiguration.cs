using Incentive.Core.Entities.IncentivePlan;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Incentive.Infrastructure.Data.EntityConfigurations
{
    public class TieredIncentiveTierConfiguration : IEntityTypeConfiguration<TieredIncentiveTier>
    {
        public void Configure(EntityTypeBuilder<TieredIncentiveTier> builder)
        {
            // The key is already defined in the entity with [Key] attribute
            // Do not call builder.HasKey() here

            builder.ToTable("TieredIncentiveTiers", "IncentiveManagement");

            builder.Property(t => t.FromValue)
                .IsRequired()
                .HasColumnType("decimal(18, 2)");

            builder.Property(t => t.ToValue)
                .IsRequired()
                .HasColumnType("decimal(18, 2)");

            builder.Property(t => t.IncentiveValue)
                .IsRequired()
                .HasColumnType("decimal(18, 2)");

            builder.Property(t => t.CalculationType)
                .IsRequired()
                .HasConversion<string>();

            // Configure relationships
            builder.HasOne(t => t.TieredIncentivePlan)
                .WithMany(p => p.Tiers)
                .HasForeignKey(t => t.TieredIncentivePlanId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
