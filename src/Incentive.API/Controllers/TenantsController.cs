using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Incentive.Core.Entities;
using Incentive.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Incentive.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class TenantsController : ControllerBase
    {
        private readonly ITenantService _tenantService;

        public TenantsController(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tenant>>> GetAllTenants()
        {
            var tenants = await _tenantService.GetAllTenantsAsync();
            return Ok(tenants);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Tenant>> GetTenant(string id)
        {
            var tenant = await _tenantService.GetTenantAsync(id);
            if (tenant == null)
            {
                return NotFound();
            }

            return Ok(tenant);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<Tenant>> CreateTenant([FromBody] Tenant tenant)
        {
            var exists = await _tenantService.TenantExistsAsync(tenant.Identifier);
            if (exists)
            {
                return BadRequest($"Tenant with identifier '{tenant.Identifier}' already exists");
            }

            var newTenant = await _tenantService.CreateTenantAsync(tenant.Name, tenant.Identifier, tenant.ConnectionString);
            return CreatedAtAction(nameof(GetTenant), new { id = newTenant.Id }, newTenant);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTenant(string id, [FromBody] Tenant tenant)
        {
            var existingTenant = await _tenantService.GetTenantAsync(id);
            if (existingTenant == null)
            {
                return NotFound();
            }

            tenant.Id = id;
            var success = await _tenantService.UpdateTenantAsync(tenant);
            if (!success)
            {
                return BadRequest("Failed to update tenant");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTenant(string id)
        {
            var tenant = await _tenantService.GetTenantAsync(id);
            if (tenant == null)
            {
                return NotFound();
            }

            var success = await _tenantService.DeleteTenantAsync(id);
            if (!success)
            {
                return BadRequest("Failed to delete tenant");
            }

            return NoContent();
        }

        [HttpGet("current")]
        public ActionResult<string> GetCurrentTenant()
        {
            var tenantId = _tenantService.GetCurrentTenantId();
            return Ok(tenantId);
        }
    }
}
