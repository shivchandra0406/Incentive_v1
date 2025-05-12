using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Incentive.Core.Entities;

namespace Incentive.Core.Interfaces
{
    public interface ITenantService
    {
        Task<Tenant> GetTenantAsync(string tenantId);
        Task<IEnumerable<Tenant>> GetAllTenantsAsync();
        Task<Tenant> CreateTenantAsync(string id,string name, string identifier, string connectionString);
        Task<bool> UpdateTenantAsync(Tenant tenant);
        Task<bool> DeleteTenantAsync(string tenantId);
        Task<bool> TenantExistsAsync(string identifier);
        string GetCurrentTenantId();
    }
}
