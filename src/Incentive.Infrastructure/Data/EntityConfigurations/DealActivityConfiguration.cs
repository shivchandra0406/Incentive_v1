using Incentive.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Incentive.Infrastructure.Data.EntityConfigurations
{
    public class DealActivityConfiguration : IEntityTypeConfiguration<DealActivity>
    {
        public void Configure(EntityTypeBuilder<DealActivity> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Type)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(a => a.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(a => a.Notes)
                .HasMaxLength(1000);

            builder.Property(a => a.ActivityDate)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Configure relationships
            builder.HasOne(a => a.Deal)
                .WithMany(d => d.Activities)
                .HasForeignKey(a => a.DealId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
