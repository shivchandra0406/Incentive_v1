using Incentive.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Incentive.Infrastructure.Data.EntityConfigurations
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToTable("Projects", "IncentiveManagement");

            builder.HasKey(p => p.Id);


            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Description)
                .HasMaxLength(500);

            builder.Property(p => p.Location)
                .HasMaxLength(255);

            builder.Property(p => p.PropertyType)
                .HasMaxLength(50);

            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.Area)
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.Status)
                .HasMaxLength(50)
                .HasDefaultValue("For Sale");

            builder.Property(p => p.AgentName)
                .HasMaxLength(100);

            builder.Property(p => p.AgentContact)
                .HasMaxLength(200);

            builder.Property(p => p.ImagesMedia)
                .HasMaxLength(1000);

            builder.Property(p => p.Amenities)
                .HasMaxLength(500);

            builder.Property(p => p.OwnershipDetails)
                .HasMaxLength(50);

            builder.Property(p => p.MLSListingId)
                .HasMaxLength(50);

            builder.Property(p => p.TotalValue)
                .HasColumnType("decimal(18,2)");

            builder.Property(p => p.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(p => p.TenantId)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasQueryFilter(p => !p.IsDeleted);
        }
    }
}
