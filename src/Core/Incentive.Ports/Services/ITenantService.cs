using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Incentive.Domain.Entities;

namespace Incentive.Ports.Services
{
    public interface ITenantService
    {
        Task<Tenant> GetTenantAsync(string tenantId);
        Task<IEnumerable<Tenant>> GetAllTenantsAsync();
        Task<Tenant> CreateTenantAsync(string name, string identifier, string connectionString);
        Task<bool> UpdateTenantAsync(Tenant tenant);
        Task<bool> DeleteTenantAsync(string tenantId);
        Task<bool> TenantExistsAsync(string identifier);
        string GetCurrentTenantId();
    }
}
