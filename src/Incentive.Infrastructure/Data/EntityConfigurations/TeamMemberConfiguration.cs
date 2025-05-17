using Incentive.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Incentive.Infrastructure.Data.EntityConfigurations
{
    public class TeamMemberConfiguration : IEntityTypeConfiguration<TeamMember>
    {
        public void Configure(EntityTypeBuilder<TeamMember> builder)
        {
            builder.HasKey(tm => tm.Id);

            builder.Property(tm => tm.UserId)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(tm => tm.Role)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(tm => tm.IsActive)
                .IsRequired();

            builder.Property(tm => tm.JoinedDate)
                .IsRequired();

            // Configure relationships
            builder.HasOne(tm => tm.Team)
                .WithMany(t => t.Members)
                .HasForeignKey(tm => tm.TeamId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
