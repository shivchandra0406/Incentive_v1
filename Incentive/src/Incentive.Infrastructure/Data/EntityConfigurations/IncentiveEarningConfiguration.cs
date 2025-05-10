using Incentive.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Incentive.Infrastructure.Data.EntityConfigurations
{
    public class IncentiveEarningConfiguration : IEntityTypeConfiguration<IncentiveEarning>
    {
        public void Configure(EntityTypeBuilder<IncentiveEarning> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(i => i.EarningDate)
                .IsRequired();

            builder.Property(i => i.Status)
                .IsRequired()
                .HasConversion<string>();

            // Configure relationships
            builder.HasOne(i => i.Booking)
                .WithOne(b => b.IncentiveEarning)
                .HasForeignKey<IncentiveEarning>(i => i.BookingId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(i => i.Salesperson)
                .WithMany(s => s.IncentiveEarnings)
                .HasForeignKey(i => i.SalespersonId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(i => i.IncentiveRule)
                .WithMany(r => r.IncentiveEarnings)
                .HasForeignKey(i => i.IncentiveRuleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
