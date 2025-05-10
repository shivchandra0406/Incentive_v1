using Incentive.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Incentive.Infrastructure.Data.EntityConfigurations
{
    public class SalespersonConfiguration : IEntityTypeConfiguration<Salesperson>
    {
        public void Configure(EntityTypeBuilder<Salesperson> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(s => s.LastName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(s => s.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.PhoneNumber)
                .HasMaxLength(20);

            builder.Property(s => s.EmployeeId)
                .HasMaxLength(20);

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
                .WithOne(i => i.Salesperson)
                .HasForeignKey(i => i.SalespersonId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
