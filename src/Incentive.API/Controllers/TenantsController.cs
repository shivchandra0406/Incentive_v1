using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Incentive.API.Attributes;
using Incentive.Application.Common.Models;
using Incentive.Core.Entities;
using Incentive.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Incentive.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    [RequiresTenantId(isRequired: false, description: "Not required for tenant management endpoints")]
    public class TenantsController : ControllerBase
    {
        private readonly ITenantService _tenantService;

        public TenantsController(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponse<IEnumerable<Tenant>>>> GetAllTenants()
        {
            var tenants = await _tenantService.GetAllTenantsAsync();
            return Ok(BaseResponse<IEnumerable<Tenant>>.Success(tenants));
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
        [AllowAnonymous]
        public async Task<ActionResult<BaseResponse<Tenant>>> CreateTenant([FromBody] Tenant tenant)
        {
            var exists = await _tenantService.TenantExistsAsync(tenant.Identifier);
            if (exists)
            {
                return BadRequest(BaseResponse<Tenant>.Failure($"Tenant with identifier '{tenant.Identifier}' already exists"));
            }

            var newTenant = await _tenantService.CreateTenantAsync(tenant.Id, tenant.Name, tenant.Identifier, tenant.ConnectionString);
            return CreatedAtAction(nameof(GetTenant), new { id = newTenant.Id }, BaseResponse<Tenant>.Success(newTenant, "Tenant created successfully"));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BaseResponse<object>>> UpdateTenant(string id, [FromBody] Tenant tenant)
        {
            var existingTenant = await _tenantService.GetTenantAsync(id);
            if (existingTenant == null)
            {
                return NotFound(BaseResponse<object>.Failure($"Tenant with ID {id} not found"));
            }

            tenant.Id = id;
            var success = await _tenantService.UpdateTenantAsync(tenant);
            if (!success)
            {
                return BadRequest(BaseResponse<object>.Failure("Failed to update tenant"));
            }

            return Ok(BaseResponse<object>.Success(new {}, "Tenant updated successfully"));
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

            return Ok(BaseResponse<object>.Success(new {}, "Tenant deleted successfully"));
        }

        [HttpGet("current")]
        public ActionResult<BaseResponse<string>> GetCurrentTenant()
        {
            var tenantId = _tenantService.GetCurrentTenantId();
            return Ok(BaseResponse<string>.Success(tenantId));
        }
    }
}
