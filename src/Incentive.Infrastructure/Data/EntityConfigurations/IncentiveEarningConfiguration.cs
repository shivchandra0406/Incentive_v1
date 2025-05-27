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
            builder.HasOne(i => i.Deal)
                .WithMany(d => d.IncentiveEarnings)
                .HasForeignKey(i => i.DealId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
