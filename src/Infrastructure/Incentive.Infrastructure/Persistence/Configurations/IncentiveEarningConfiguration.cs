using Incentive.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Incentive.Infrastructure.Persistence.Configurations
{
    public class IncentiveEarningConfiguration : IEntityTypeConfiguration<IncentiveEarning>
    {
        public void Configure(EntityTypeBuilder<IncentiveEarning> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(e => e.EarningDate)
                .IsRequired();

            builder.Property(e => e.Status)
                .IsRequired();

            // Configure relationships
            builder.HasOne(e => e.Booking)
                .WithOne(b => b.IncentiveEarning)
                .HasForeignKey<IncentiveEarning>(e => e.BookingId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Salesperson)
                .WithMany(s => s.IncentiveEarnings)
                .HasForeignKey(e => e.SalespersonId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.IncentiveRule)
                .WithMany(r => r.IncentiveEarnings)
                .HasForeignKey(e => e.IncentiveRuleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
