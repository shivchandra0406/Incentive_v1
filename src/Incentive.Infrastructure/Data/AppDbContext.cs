using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Incentive.Core.Common;
using Incentive.Core.Entities;
using Incentive.Infrastructure.Identity;
using Incentive.Infrastructure.MultiTenancy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Incentive.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        private readonly ITenantProvider _tenantProvider;
        private readonly ICurrentUserService _currentUserService;

        public AppDbContext(
            DbContextOptions<AppDbContext> options,
            ITenantProvider tenantProvider,
            ICurrentUserService currentUserService) : base(options)
        {
            _tenantProvider = tenantProvider;
            _currentUserService = currentUserService;
        }

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Salesperson> Salespeople { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<IncentiveRule> IncentiveRules { get; set; }
        public DbSet<IncentiveEarning> IncentiveEarnings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Set default schema for Identity tables
            builder.HasDefaultSchema("dbo");

            // Configure schemas for different entity types
            builder.Entity<AppUser>().ToTable("Users", "Identity");
            builder.Entity<AppRole>().ToTable("Roles", "Identity");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "Identity");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "Identity");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "Identity");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "Identity");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", "Identity");

            // Configure schemas for business entities
            builder.Entity<Tenant>().ToTable("Tenants", "Tenant");

            // IncentiveManagement schema
            builder.Entity<Salesperson>().ToTable("Salespeople", "IncentiveManagement");
            builder.Entity<Project>().ToTable("Projects", "IncentiveManagement");
            builder.Entity<Booking>().ToTable("Bookings", "IncentiveManagement");
            builder.Entity<IncentiveRule>().ToTable("IncentiveRules", "IncentiveManagement");
            builder.Entity<IncentiveEarning>().ToTable("IncentiveEarnings", "IncentiveManagement");

            // Apply entity configurations
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            // Add global query filters for soft-deletable entities
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (typeof(SoftDeletableEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = System.Linq.Expressions.Expression.Parameter(entityType.ClrType, "e");
                    var isDeletedProperty = System.Linq.Expressions.Expression.Property(parameter, "IsDeleted");
                    var falseConstant = System.Linq.Expressions.Expression.Constant(false);
                    var notDeletedComparison = System.Linq.Expressions.Expression.Equal(isDeletedProperty, falseConstant);
                    var lambda = System.Linq.Expressions.Expression.Lambda(notDeletedComparison, parameter);

                    builder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Get current tenant ID
            var tenantId = _tenantProvider.GetTenantId();
            var userId = _currentUserService.GetUserId();
            var now = DateTime.UtcNow;

            // Apply audit information
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = now;
                        entry.Entity.CreatedBy = userId;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedAt = now;
                        entry.Entity.LastModifiedBy = userId;
                        break;
                }
            }

            // Apply soft delete
            foreach (var entry in ChangeTracker.Entries<SoftDeletableEntity>())
            {
                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedAt = now;
                    entry.Entity.DeletedBy = userId;
                }
            }

            // Apply multi-tenancy
            foreach (var entry in ChangeTracker.Entries<MultiTenantEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.TenantId = tenantId;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
