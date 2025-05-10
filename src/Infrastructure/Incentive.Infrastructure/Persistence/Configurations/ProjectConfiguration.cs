using Incentive.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Incentive.Infrastructure.Persistence.Configurations
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Description)
                .HasMaxLength(500);

            builder.Property(p => p.Location)
                .HasMaxLength(255);

            builder.Property(p => p.StartDate)
                .IsRequired();

            builder.Property(p => p.TotalValue)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            // Configure relationships
            builder.HasMany(p => p.Bookings)
                .WithOne(b => b.Project)
                .HasForeignKey(b => b.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.IncentiveRules)
                .WithOne(r => r.Project)
                .HasForeignKey(r => r.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
