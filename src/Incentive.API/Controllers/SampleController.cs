using Incentive.API.Attributes;
using Incentive.API.Extensions;
using Incentive.Core.Entities;
using Incentive.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Incentive.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [RequiresTenantId(description: "The tenant ID to access data from")]
    public class SampleController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<SampleController> _logger;

        public SampleController(AppDbContext dbContext, ILogger<SampleController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Gets information about the current tenant
        /// </summary>
        /// <returns>The tenant information</returns>
        [HttpGet("tenant-info")]
        public IActionResult GetTenantInfo()
        {
            // Get tenant ID from request
            var tenantId = this.GetTenantId();

            // Log tenant ID
            _logger.LogInformation("Tenant ID: {TenantId}", tenantId);

            // Return tenant ID
            return Ok(new { TenantId = tenantId });
        }

        /// <summary>
        /// Gets all deals for the current tenant
        /// </summary>
        /// <returns>A list of deals</returns>
        [HttpGet("deals")]
        public async Task<ActionResult<IEnumerable<Deal>>> GetDeals()
        {
            // Get tenant ID from request
            var tenantId = this.GetTenantId();

            // Log tenant ID
            _logger.LogInformation("Getting deals for tenant: {TenantId}", tenantId);

            // Get deals for tenant (tenant filter is automatically applied)
            var deals = await _dbContext.Deals.ToListAsync();

            return Ok(deals);
        }

        /// <summary>
        /// Creates a new deal for the current tenant
        /// </summary>
        /// <param name="deal">The deal to create</param>
        /// <returns>The created deal</returns>
        [HttpPost("deals")]
        public async Task<ActionResult<Deal>> CreateDeal(Deal deal)
        {
            // Get tenant ID from request
            var tenantId = this.GetTenantId();

            // Log tenant ID
            _logger.LogInformation("Creating deal for tenant: {TenantId}", tenantId);

            // Add deal (tenant ID is automatically set)
            await _dbContext.Deals.AddAsync(deal);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDeals), new { id = deal.Id }, deal);
        }
        
        /// <summary>
        /// This endpoint does not require a tenant ID
        /// </summary>
        /// <returns>A message</returns>
        [HttpGet("no-tenant")]
        [RequiresTenantId(isRequired: false, description: "Optional tenant ID")]
        public IActionResult GetWithoutTenant()
        {
            return Ok(new { Message = "This endpoint does not require a tenant ID" });
        }
    }
}
