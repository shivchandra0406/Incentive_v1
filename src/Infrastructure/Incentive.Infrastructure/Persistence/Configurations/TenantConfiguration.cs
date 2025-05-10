using Incentive.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Incentive.Infrastructure.Persistence.Configurations
{
    public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(t => t.Identifier)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(t => t.Identifier)
                .IsUnique();

            builder.Property(t => t.ConnectionString)
                .HasMaxLength(500);

            builder.Property(t => t.IsActive)
                .IsRequired()
                .HasDefaultValue(true);
        }
    }
}
