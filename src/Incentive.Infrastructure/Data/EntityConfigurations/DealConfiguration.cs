using Incentive.Core.Entities;
using Incentive.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Incentive.Infrastructure.Data.EntityConfigurations
{
    public class DealConfiguration : IEntityTypeConfiguration<Deal>
    {
        public void Configure(EntityTypeBuilder<Deal> builder)
        {
            builder.HasKey(d => d.Id);

            builder.Property(d => d.DealName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(d => d.CustomerName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(d => d.CustomerEmail)
                .HasMaxLength(100);

            builder.Property(d => d.CustomerPhone)
                .HasMaxLength(50);

            builder.Property(d => d.CustomerAddress)
                .HasMaxLength(500);

            builder.Property(d => d.TotalAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(d => d.PaidAmount)
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0);

            builder.Property(d => d.RemainingAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(d => d.CurrencyType)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(d => d.TaxPercentage)
                .HasColumnType("decimal(5,2)")
                .HasDefaultValue(0);

            builder.Property(d => d.TaxAmount)
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0);

            builder.Property(d => d.DiscountAmount)
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0);

            builder.Property(d => d.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(d => d.DealDate)
                .IsRequired();

            builder.Property(d => d.ReferralName)
                .HasMaxLength(200);

            builder.Property(d => d.ReferralEmail)
                .HasMaxLength(100);

            builder.Property(d => d.ReferralPhone)
                .HasMaxLength(50);

            builder.Property(d => d.ReferralCommission)
                .HasColumnType("decimal(18,2)");

            builder.Property(d => d.Source)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(d => d.Notes)
                .HasMaxLength(2000);

            // No Project or Salesperson relationships

            builder.HasOne(d => d.AssignedRule)
                .WithMany(r => r.Deals)
                .HasForeignKey(d => d.IncentiveRuleId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure relationships with child entities
            builder.HasMany(d => d.Payments)
                .WithOne(p => p.Deal)
                .HasForeignKey(p => p.DealId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(d => d.Activities)
                .WithOne(a => a.Deal)
                .HasForeignKey(a => a.DealId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
