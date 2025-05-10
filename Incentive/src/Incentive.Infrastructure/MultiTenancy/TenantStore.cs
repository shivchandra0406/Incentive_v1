using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Incentive.Core.Entities;
using Incentive.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Incentive.Infrastructure.MultiTenancy
{
    public class TenantStore
    {
        private readonly AppDbContext _dbContext;
        private readonly IMemoryCache _cache;
        private readonly ILogger<TenantStore> _logger;
        private readonly string _cacheKey = "Tenants";
        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(10);

        public TenantStore(
            AppDbContext dbContext,
            IMemoryCache cache,
            ILogger<TenantStore> logger)
        {
            _dbContext = dbContext;
            _cache = cache;
            _logger = logger;
        }

        public async Task<Tenant> GetTenantAsync(string tenantId)
        {
            try
            {
                var tenants = await GetAllTenantsAsync();
                return tenants.FirstOrDefault(t => t.Id.ToString() == tenantId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting tenant with ID {TenantId}", tenantId);
                return null;
            }
        }

        public async Task<Tenant> GetTenantByIdentifierAsync(string identifier)
        {
            try
            {
                var tenants = await GetAllTenantsAsync();
                return tenants.FirstOrDefault(t => t.Identifier == identifier);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting tenant with identifier {TenantIdentifier}", identifier);
                return null;
            }
        }

        public async Task<IEnumerable<Tenant>> GetAllTenantsAsync()
        {
            try
            {
                // Try to get from cache first
                if (_cache.TryGetValue(_cacheKey, out IEnumerable<Tenant> cachedTenants))
                {
                    return cachedTenants;
                }

                // If not in cache, get from database
                var tenants = await _dbContext.Tenants.ToListAsync();
                
                // Store in cache
                _cache.Set(_cacheKey, tenants, _cacheDuration);
                
                return tenants;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all tenants");
                return Enumerable.Empty<Tenant>();
            }
        }

        public async Task<bool> TenantExistsAsync(string identifier)
        {
            try
            {
                var tenants = await GetAllTenantsAsync();
                return tenants.Any(t => t.Identifier == identifier);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if tenant exists with identifier {TenantIdentifier}", identifier);
                return false;
            }
        }

        public void ClearCache()
        {
            _cache.Remove(_cacheKey);
        }
    }
}
