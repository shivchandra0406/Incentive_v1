using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Incentive.Core.Entities;
using Incentive.Core.Interfaces;
using Incentive.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Incentive.Infrastructure.MultiTenancy
{
    public class TenantService : ITenantService
    {
        private readonly AppDbContext _dbContext;
        private readonly ITenantProvider _tenantProvider;
        private readonly ILogger<TenantService> _logger;

        public TenantService(
            AppDbContext dbContext,
            ITenantProvider tenantProvider,
            ILogger<TenantService> logger)
        {
            _dbContext = dbContext;
            _tenantProvider = tenantProvider;
            _logger = logger;
        }

        public async Task<Tenant> GetTenantAsync(string tenantId)
        {
            try
            {
                return await _dbContext.Tenants.FindAsync(Guid.Parse(tenantId)) ?? null!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting tenant with ID {TenantId}", tenantId);
                return null!;
            }
        }

        public async Task<IEnumerable<Tenant>> GetAllTenantsAsync()
        {
            try
            {
                return await _dbContext.Tenants.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all tenants");
                return [];
            }
        }

        public async Task<Tenant> CreateTenantAsync(string id, string name, string identifier, string connectionString)
        {
            try
            {
                var tenant = new Tenant
                {
                    Id = id,
                    Name = name,
                    Identifier = identifier,
                    ConnectionString = connectionString,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                await _dbContext.Tenants.AddAsync(tenant);
                await _dbContext.SaveChangesAsync();

                return tenant;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating tenant with name {TenantName} and identifier {TenantIdentifier}", name, identifier);
                return null!;
            }
        }

        public async Task<bool> UpdateTenantAsync(Tenant tenant)
        {
            try
            {
                var existingTenant = await _dbContext.Tenants.FindAsync(tenant.Id);
                if (existingTenant == null)
                {
                    return false;
                }

                existingTenant.Name = tenant.Name;
                existingTenant.ConnectionString = tenant.ConnectionString;
                existingTenant.IsActive = tenant.IsActive;

                _dbContext.Tenants.Update(existingTenant);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating tenant with ID {TenantId}", tenant.Id);
                return false;
            }
        }

        public async Task<bool> DeleteTenantAsync(string tenantId)
        {
            try
            {
                var tenant = await _dbContext.Tenants.FindAsync(Guid.Parse(tenantId));
                if (tenant == null)
                {
                    return false;
                }

                _dbContext.Tenants.Remove(tenant);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting tenant with ID {TenantId}", tenantId);
                return false;
            }
        }

        public async Task<bool> TenantExistsAsync(string identifier)
        {
            try
            {
                return await _dbContext.Tenants.AnyAsync(t => t.Identifier == identifier);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if tenant exists with identifier {TenantIdentifier}", identifier);
                return false;
            }
        }

        public string GetCurrentTenantId()
        {
            return _tenantProvider.GetTenantId();
        }
    }
}
