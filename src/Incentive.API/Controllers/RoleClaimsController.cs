using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Incentive.Application.DTOs;
using Incentive.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Incentive.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "RequireAdminRole")]
    public class RoleClaimsController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly ILogger<RoleClaimsController> _logger;

        public RoleClaimsController(
            IIdentityService identityService,
            ILogger<RoleClaimsController> logger)
        {
            _identityService = identityService;
            _logger = logger;
        }

        [HttpGet("{roleId}")]
        public async Task<ActionResult<IEnumerable<RoleClaimDto>>> GetRoleClaims(string roleId)
        {
            try
            {
                var role = await _identityService.GetRoleByIdAsync(roleId);

                if (role == null)
                {
                    return NotFound("Role not found");
                }

                var claims = await _identityService.GetRoleClaimsAsync(roleId);
                var claimDtos = claims.Select(c => new RoleClaimDto
                {
                    RoleId = roleId,
                    ClaimType = c.Type,
                    ClaimValue = c.Value
                });

                return Ok(claimDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting claims for role with ID {RoleId}", roleId);
                return StatusCode(500, "An error occurred while retrieving role claims");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddClaimToRole(CreateRoleClaimDto createRoleClaimDto)
        {
            try
            {
                var role = await _identityService.GetRoleByIdAsync(createRoleClaimDto.RoleId);

                if (role == null)
                {
                    return NotFound("Role not found");
                }

                var result = await _identityService.AddClaimToRoleAsync(
                    createRoleClaimDto.RoleId,
                    createRoleClaimDto.ClaimType,
                    createRoleClaimDto.ClaimValue);

                if (!result)
                {
                    return BadRequest("Failed to add claim to role");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding claim to role");
                return StatusCode(500, "An error occurred while adding the claim to the role");
            }
        }

        [HttpDelete("{roleId}/{claimType}/{claimValue}")]
        public async Task<IActionResult> RemoveClaimFromRole(string roleId, string claimType, string claimValue)
        {
            try
            {
                var role = await _identityService.GetRoleByIdAsync(roleId);

                if (role == null)
                {
                    return NotFound("Role not found");
                }

                var result = await _identityService.RemoveClaimFromRoleAsync(roleId, claimType, claimValue);

                if (!result)
                {
                    return BadRequest("Failed to remove claim from role");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing claim from role");
                return StatusCode(500, "An error occurred while removing the claim from the role");
            }
        }
    }
}
