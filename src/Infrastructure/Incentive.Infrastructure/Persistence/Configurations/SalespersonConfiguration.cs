using Incentive.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Incentive.Infrastructure.Persistence.Configurations
{
    public class SalespersonConfiguration : IEntityTypeConfiguration<Salesperson>
    {
        public void Configure(EntityTypeBuilder<Salesperson> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasIndex(s => new { s.Email, s.TenantId })
                .IsUnique()
                .HasFilter("\"IsDeleted\" = false");

            builder.Property(s => s.PhoneNumber)
                .HasMaxLength(20);

            builder.Property(s => s.EmployeeId)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(s => new { s.EmployeeId, s.TenantId })
                .IsUnique()
                .HasFilter("\"IsDeleted\" = false");

            builder.Property(s => s.HireDate)
                .IsRequired();

            builder.Property(s => s.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Configure relationships
            builder.HasMany(s => s.Bookings)
                .WithOne(b => b.Salesperson)
                .HasForeignKey(b => b.SalespersonId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(s => s.IncentiveEarnings)
                .WithOne(e => e.Salesperson)
                .HasForeignKey(e => e.SalespersonId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
