using Incentive.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Incentive.Infrastructure.Data.EntityConfigurations
{
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.CustomerName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(b => b.CustomerEmail)
                .HasMaxLength(100);

            builder.Property(b => b.CustomerPhone)
                .HasMaxLength(20);

            builder.Property(b => b.BookingDate)
                .IsRequired();

            builder.Property(b => b.BookingValue)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(b => b.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(b => b.Notes)
                .HasMaxLength(500);

            // Configure relationships
            builder.HasOne(b => b.Project)
                .WithMany(p => p.Bookings)
                .HasForeignKey(b => b.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Salesperson)
                .WithMany(s => s.Bookings)
                .HasForeignKey(b => b.SalespersonId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.IncentiveEarning)
                .WithOne(i => i.Booking)
                .HasForeignKey<IncentiveEarning>(i => i.BookingId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
