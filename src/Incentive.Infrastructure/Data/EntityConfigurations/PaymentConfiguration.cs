using Incentive.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Incentive.Infrastructure.Data.EntityConfigurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.PaymentDate)
                .IsRequired();

            builder.Property(p => p.PaymentMethod)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.TransactionReference)
                .HasMaxLength(100);

            builder.Property(p => p.Notes)
                .HasMaxLength(500);

            builder.Property(p => p.IsVerified)
                .IsRequired()
                .HasDefaultValue(false);

            // Configure relationships
            builder.HasOne(p => p.Deal)
                .WithMany(d => d.Payments)
                .HasForeignKey(p => p.DealId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
