using Incentive.Core.Common;
using Incentive.Core.Entities;
using Incentive.Core.Entities.IncentivePlan;
using Incentive.Infrastructure.Identity;
using Incentive.Infrastructure.MultiTenancy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using System.Reflection;

namespace Incentive.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        private readonly ITenantProvider _tenantProvider;
        private readonly ICurrentUserService _currentUserService;

        public AppDbContext(
            DbContextOptions<AppDbContext> options,
            ITenantProvider tenantProvider = null,
            ICurrentUserService currentUserService = null) : base(options)
        {
            _tenantProvider = tenantProvider;
            _currentUserService = currentUserService;
        }

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<IncentiveRule> IncentiveRules { get; set; }
        public DbSet<IncentiveEarning> IncentiveEarnings { get; set; }
        public DbSet<Deal> Deals { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<DealActivity> DealActivities { get; set; }

        // Team entities
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }

        // Incentive Plan entities
        public DbSet<IncentivePlanBase> IncentivePlans { get; set; }
        public DbSet<TargetBasedIncentivePlan> TargetBasedIncentivePlans { get; set; }
        public DbSet<RoleBasedIncentivePlan> RoleBasedIncentivePlans { get; set; }
        public DbSet<ProjectBasedIncentivePlan> ProjectBasedIncentivePlans { get; set; }
        public DbSet<KickerIncentivePlan> KickerIncentivePlans { get; set; }
        public DbSet<TieredIncentivePlan> TieredIncentivePlans { get; set; }
        public DbSet<TieredIncentiveTier> TieredIncentiveTiers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Set default schema for all tables
            builder.HasDefaultSchema("dbo");

            // Configure schemas for Identity tables (these don't use our SchemaAttribute)
            builder.Entity<AppUser>().ToTable("Users", "Identity");
            builder.Entity<AppRole>().ToTable("Roles", "Identity");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "Identity");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "Identity");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "Identity");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "Identity");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", "Identity");

            // Apply entity configurations first
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            // Apply schema attributes after configurations
            ApplySchemaAttributes(builder);

            // Add global query filters for soft-deletable entities and multi-tenancy
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                // Skip derived types (important for TPH)
                if (entityType.BaseType != null)
                    continue;

                var clrType = entityType.ClrType;
                var parameter = Expression.Parameter(clrType, "e");
                Expression finalExpression = null;

                // Multi-tenancy filter
                if (typeof(MultiTenantEntity).IsAssignableFrom(clrType))
                {
                    var tenantIdProperty = Expression.Property(parameter, nameof(MultiTenantEntity.TenantId));
                    var getCurrentTenantIdMethod = typeof(AppDbContext).GetMethod(nameof(GetCurrentTenantId),
                        BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                    if (getCurrentTenantIdMethod != null)
                    {
                        var tenantIdCall = Expression.Call(Expression.Constant(this), getCurrentTenantIdMethod);
                        var tenantFilter = Expression.Equal(tenantIdProperty, tenantIdCall);
                        finalExpression = tenantFilter;
                    }
                }

                // Soft delete
                if (typeof(SoftDeletableEntity).IsAssignableFrom(clrType))
                {
                    var isDeletedProperty = Expression.Property(parameter, nameof(SoftDeletableEntity.IsDeleted));
                    var notDeleted = Expression.Equal(isDeletedProperty, Expression.Constant(false));
                    finalExpression = notDeleted;

                }
                // Apply the combined filter
                if (finalExpression != null)
                {
                    var lambda = Expression.Lambda(finalExpression, parameter);
                    builder.Entity(clrType).HasQueryFilter(lambda);
                }
            }

        }

        /// <summary>
        /// Gets the current tenant ID from the tenant provider.
        /// </summary>
        /// <returns>The current tenant ID.</returns>
        public string GetCurrentTenantId()
        {
            return _tenantProvider?.GetTenantId() ?? "00000000-0000-0000-0000-000000000000";
        }

        /// <summary>
        /// Applies schema attributes to entity types.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        private void ApplySchemaAttributes(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Skip non-CLR types (like query types)
                if (entityType.ClrType == null)
                    continue;

                // Get the schema attribute if it exists
                var schemaAttribute = entityType.ClrType.GetCustomAttribute<SchemaAttribute>();
                if (schemaAttribute != null)
                {
                    // Get the current table name (which might have been set by a configuration)
                    var tableName = entityType.GetTableName();

                    // Apply the schema from the attribute
                    modelBuilder.Entity(entityType.ClrType)
                        .ToTable(tableName, schemaAttribute.Name);

                    Console.WriteLine($"Applied schema '{schemaAttribute.Name}' to entity {entityType.ClrType.Name}");
                }
            }
        }

        #region Save changes
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var tenantId = GetCurrentTenantId();
            var userId = _currentUserService?.GetUserId() ?? "00000000-0000-0000-0000-000000000000";
            var now = DateTime.UtcNow;
            _ = Guid.TryParse(userId, out Guid GuidUserId);

            foreach (var entry in ChangeTracker.Entries())
            {
                var count = ChangeTracker.Entries().Count();
                ApplyAuditInfo(entry, now, GuidUserId);
                ApplySoftDelete(entry, now, GuidUserId);
                ApplyMultiTenancy(entry, tenantId);
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
        private void ApplyAuditInfo(EntityEntry entry, DateTime now, Guid userId)
        {
            var entity = entry.Entity;

            if (entity is UserAuditableEntity userAuditable)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        userAuditable.CreatedAt = now;
                        userAuditable.CreatedBy = userId;
                        userAuditable.LastModifiedAt = now;
                        userAuditable.LastModifiedBy = userId;
                        break;
                    case EntityState.Modified:
                        userAuditable.LastModifiedAt = now;
                        userAuditable.LastModifiedBy = userId;
                        break;
                }
            }
            else if (entity is AuditableEntity auditable)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        auditable.CreatedAt = now;
                        auditable.LastModifiedAt = now;
                        break;
                    case EntityState.Modified:
                        auditable.LastModifiedAt = now;
                        break;
                }
            }
        }
        private void ApplySoftDelete(EntityEntry entry, DateTime now, Guid userId)
        {
            var entity = entry.Entity;

            if (entry.State != EntityState.Deleted)
                return;

            if (entity is UserAuditableEntity userDeletable)
            {
                entry.State = EntityState.Modified;
                userDeletable.IsDeleted = true;
                userDeletable.DeletedAt = now;
                userDeletable.DeletedBy = userId;
            }
            else if (entity is SoftDeletableEntity deletable)
            {
                entry.State = EntityState.Modified;
                deletable.IsDeleted = true;
                deletable.DeletedAt = now;
            }
        }
        private void ApplyMultiTenancy(EntityEntry entry, string tenantId)
        {
            if (entry.State == EntityState.Added && entry.Entity is MultiTenantEntity tenantEntity)
            {
                tenantEntity.TenantId = tenantId;
            }
        }
        #endregion
    }
}
