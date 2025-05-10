using System.Threading.Tasks;
using Incentive.Application.Common.Models;
using Incentive.Domain.Entities;
using Incentive.Ports.Services;
using Incentive.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Incentive.WebAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TenantController : ApiController
    {
        private readonly ITenantService _tenantService;
        private readonly IIdentityService _identityService;

        public TenantController(ITenantService tenantService, IIdentityService identityService)
        {
            _tenantService = tenantService;
            _identityService = identityService;
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponse<object>>> GetAllTenants()
        {
            var tenants = await _tenantService.GetAllTenantsAsync();
            return Ok(BaseResponse<object>.Success(tenants));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse<Tenant>>> GetTenant(string id)
        {
            var tenant = await _tenantService.GetTenantAsync(id);
            if (tenant == null)
            {
                return NotFound(BaseResponse<Tenant>.Failure($"Tenant with ID {id} not found"));
            }

            return Ok(BaseResponse<Tenant>.Success(tenant));
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse<Tenant>>> CreateTenant(CreateTenantRequest request)
        {
            if (await _tenantService.TenantExistsAsync(request.Identifier))
            {
                return BadRequest(BaseResponse<Tenant>.Failure($"Tenant with identifier {request.Identifier} already exists"));
            }

            var tenant = await _tenantService.CreateTenantAsync(request.Name, request.Identifier, request.ConnectionString);
            if (tenant == null)
            {
                return BadRequest(BaseResponse<Tenant>.Failure("Failed to create tenant"));
            }

            // Create admin user for the tenant
            var result = await _identityService.CreateUserAsync(
                request.AdminUserName,
                request.AdminEmail,
                request.AdminPassword,
                request.AdminFirstName,
                request.AdminLastName,
                tenant.Id.ToString());

            if (!result.Succeeded)
            {
                await _tenantService.DeleteTenantAsync(tenant.Id.ToString());
                return BadRequest(BaseResponse<Tenant>.Failure($"Failed to create admin user: {result.Message}"));
            }

            await _identityService.AddUserToRoleAsync(result.UserId, "Admin");

            return CreatedAtAction(nameof(GetTenant), new { id = tenant.Id }, BaseResponse<Tenant>.Success(tenant));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BaseResponse<object>>> UpdateTenant(string id, UpdateTenantRequest request)
        {
            var tenant = await _tenantService.GetTenantAsync(id);
            if (tenant == null)
            {
                return NotFound(BaseResponse<object>.Failure($"Tenant with ID {id} not found"));
            }

            tenant.Name = request.Name;
            tenant.ConnectionString = request.ConnectionString;

            var success = await _tenantService.UpdateTenantAsync(tenant);
            if (!success)
            {
                return BadRequest(BaseResponse<object>.Failure("Failed to update tenant"));
            }

            return Ok(BaseResponse<object>.Success(null, "Tenant updated successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BaseResponse<object>>> DeleteTenant(string id)
        {
            var tenant = await _tenantService.GetTenantAsync(id);
            if (tenant == null)
            {
                return NotFound(BaseResponse<object>.Failure($"Tenant with ID {id} not found"));
            }

            var success = await _tenantService.DeleteTenantAsync(id);
            if (!success)
            {
                return BadRequest(BaseResponse<object>.Failure("Failed to delete tenant"));
            }

            return Ok(BaseResponse<object>.Success(null, "Tenant deleted successfully"));
        }
    }
}
