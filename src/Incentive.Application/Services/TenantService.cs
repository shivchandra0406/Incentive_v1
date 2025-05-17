using Incentive.Application.Interfaces;
using Incentive.Infrastructure.MultiTenancy;

namespace Incentive.Application.Services
{
    public class TenantService : ITenantService
    {
        private readonly ITenantProvider _tenantProvider;

        public TenantService(ITenantProvider tenantProvider)
        {
            _tenantProvider = tenantProvider;
        }

        public string GetCurrentTenantId()
        {
            return _tenantProvider.GetTenantId();
        }
    }
}
