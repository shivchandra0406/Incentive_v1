using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Incentive.Application.Common.Models;
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
    public class RolesController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly ITenantService _tenantService;
        private readonly ILogger<RolesController> _logger;

        public RolesController(
            IIdentityService identityService,
            ITenantService tenantService,
            ILogger<RolesController> logger)
        {
            _identityService = identityService;
            _tenantService = tenantService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetRoles()
        {
            try
            {
                var roles = await _identityService.GetAllRolesAsync();
                var roleDtos = roles.Select(r => new RoleDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    TenantId = r.TenantId,
                    CreatedAt = r.CreatedAt,
                    CreatedBy = r.CreatedBy,
                    LastModifiedAt = r.LastModifiedAt,
                    LastModifiedBy = r.LastModifiedBy
                });

                return Ok(roleDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting roles");
                return StatusCode(500, "An error occurred while retrieving roles");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoleDto>> GetRole(string id)
        {
            try
            {
                var role = await _identityService.GetRoleByIdAsync(id);

                if (role == null)
                {
                    return NotFound();
                }

                var roleDto = new RoleDto
                {
                    Id = role.Id,
                    Name = role.Name,
                    Description = role.Description,
                    TenantId = role.TenantId,
                    CreatedAt = role.CreatedAt,
                    CreatedBy = role.CreatedBy,
                    LastModifiedAt = role.LastModifiedAt,
                    LastModifiedBy = role.LastModifiedBy
                };

                return Ok(roleDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting role with ID {RoleId}", id);
                return StatusCode(500, "An error occurred while retrieving the role");
            }
        }

        [HttpPost]
        public async Task<ActionResult<RoleDto>> CreateRole(CreateRoleDto createRoleDto)
        {
            try
            {
                var tenantId = _tenantService.GetCurrentTenantId();
                var result = await _identityService.CreateRoleAsync(createRoleDto.Name, createRoleDto.Description, tenantId);

                if (!result.Succeeded)
                {
                    return BadRequest(result.Message);
                }

                var role = await _identityService.GetRoleByIdAsync(result.RoleId);
                var roleDto = new RoleDto
                {
                    Id = role.Id,
                    Name = role.Name,
                    Description = role.Description,
                    TenantId = role.TenantId,
                    CreatedAt = role.CreatedAt,
                    CreatedBy = role.CreatedBy,
                    LastModifiedAt = role.LastModifiedAt,
                    LastModifiedBy = role.LastModifiedBy
                };

                return CreatedAtAction(nameof(GetRole), new { id = role.Id }, roleDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating role");
                return StatusCode(500, "An error occurred while creating the role");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(string id, UpdateRoleDto updateRoleDto)
        {
            try
            {
                var role = await _identityService.GetRoleByIdAsync(id);

                if (role == null)
                {
                    return NotFound();
                }

                var result = await _identityService.UpdateRoleAsync(id, updateRoleDto.Name, updateRoleDto.Description);

                if (!result)
                {
                    return BadRequest("Failed to update role");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating role with ID {RoleId}", id);
                return StatusCode(500, "An error occurred while updating the role");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            try
            {
                var role = await _identityService.GetRoleByIdAsync(id);

                if (role == null)
                {
                    return NotFound();
                }

                var result = await _identityService.DeleteRoleAsync(id);

                if (!result)
                {
                    return BadRequest("Failed to delete role");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting role with ID {RoleId}", id);
                return StatusCode(500, "An error occurred while deleting the role");
            }
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignRoleToUser(AssignRoleDto assignRoleDto)
        {
            try
            {
                var result = await _identityService.AddUserToRoleAsync(assignRoleDto.UserId, assignRoleDto.RoleName);

                if (!result)
                {
                    return BadRequest("Failed to assign role to user");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning role to user");
                return StatusCode(500, "An error occurred while assigning the role to the user");
            }
        }

        [HttpPost("remove")]
        public async Task<IActionResult> RemoveRoleFromUser(AssignRoleDto assignRoleDto)
        {
            try
            {
                var result = await _identityService.RemoveUserFromRoleAsync(assignRoleDto.UserId, assignRoleDto.RoleName);

                if (!result)
                {
                    return BadRequest("Failed to remove role from user");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing role from user");
                return StatusCode(500, "An error occurred while removing the role from the user");
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<string>>> GetUserRoles(string userId)
        {
            try
            {
                var roles = await _identityService.GetUserRolesAsync(userId);
                return Ok(roles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting roles for user with ID {UserId}", userId);
                return StatusCode(500, "An error occurred while retrieving the user's roles");
            }
        }

        [HttpPost("with-claims")]
        public async Task<ActionResult<BaseResponse<RoleWithClaimsDto>>> CreateRoleWithClaims(CreateRoleWithClaimsDto createDto)
        {
            try
            {
                var tenantId = _tenantService.GetCurrentTenantId();

                // Convert PermissionDto to Claims
                var claims = createDto.Permissions.Select(p => new System.Security.Claims.Claim("Permission", p.ClaimValue)).ToList();

                var result = await _identityService.CreateRoleWithClaimsAsync(
                    createDto.Name,
                    createDto.Description,
                    tenantId,
                    claims);

                if (!result.Succeeded)
                {
                    return BadRequest(BaseResponse<RoleWithClaimsDto>.Failure(result.Message));
                }

                // Get the created role
                var role = await _identityService.GetRoleByIdAsync(result.RoleId);

                // Get the role claims
                var roleClaims = await _identityService.GetRoleClaimsAsync(result.RoleId);

                // Map to permission DTOs
                var permissionDtos = roleClaims
                    .Where(c => c.Type == "Permission")
                    .Select(c => new PermissionDto
                    {
                        ClaimType = c.Type,
                        ClaimValue = c.Value
                    })
                    .ToList();

                // Create response DTO
                var responseDto = new RoleWithClaimsDto
                {
                    Id = role.Id,
                    Name = role.Name,
                    Description = role.Description,
                    TenantId = role.TenantId,
                    Permissions = permissionDtos,
                    CreatedAt = role.CreatedAt,
                    CreatedBy = role.CreatedBy,
                    LastModifiedAt = role.LastModifiedAt,
                    LastModifiedBy = role.LastModifiedBy
                };

                return Ok(BaseResponse<RoleWithClaimsDto>.Success(responseDto, "Role created successfully with claims"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating role with claims");
                return StatusCode(500, BaseResponse<RoleWithClaimsDto>.Failure("An error occurred while creating the role with claims"));
            }
        }
    }
}
