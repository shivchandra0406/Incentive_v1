using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Incentive.API.Attributes;
using Incentive.Application.Common.Models;
using Incentive.Application.DTOs;
using Incentive.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Incentive.API.Controllers
{
    [ApiController]
    [Route("api/roles")]
    [Authorize(Policy = "RequireAdminRole")]
    [RequiresTenantId(description: "The tenant ID to access role data")]
    public class RolePermissionsController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly ITenantService _tenantService;
        private readonly ILogger<RolePermissionsController> _logger;

        public RolePermissionsController(
            IIdentityService identityService,
            ITenantService tenantService,
            ILogger<RolePermissionsController> logger)
        {
            _identityService = identityService;
            _tenantService = tenantService;
            _logger = logger;
        }

        [HttpGet("permissions")]
        public async Task<ActionResult<BaseResponse<Dictionary<string, List<PermissionDto>>>>> GetAllRolePermissions()
        {
            try
            {
                var rolePermissions = await _identityService.GetAllRolePermissionsAsync();
                var result = new Dictionary<string, List<PermissionDto>>();

                foreach (var role in rolePermissions)
                {
                    result[role.Key] = role.Value.Select(c => new PermissionDto
                    {
                        ClaimType = c.Type,
                        ClaimValue = c.Value
                    }).ToList();
                }

                return Ok(BaseResponse<Dictionary<string, List<PermissionDto>>>.Success(result, "Role permissions retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all role permissions");
                return StatusCode(500, BaseResponse<Dictionary<string, List<PermissionDto>>>.Failure("An error occurred while retrieving role permissions"));
            }
        }

        [HttpGet("{roleName}/permissions")]
        public async Task<ActionResult<BaseResponse<List<PermissionDto>>>> GetPermissionsByRole(string roleName)
        {
            try
            {
                var permissions = await _identityService.GetRolePermissionsByNameAsync(roleName);
                
                if (permissions.Count == 0)
                {
                    var role = await _identityService.GetRoleByNameAsync(roleName);
                    if (role == null)
                    {
                        return NotFound(BaseResponse<List<PermissionDto>>.Failure($"Role '{roleName}' not found"));
                    }
                }

                var permissionDtos = permissions.Select(c => new PermissionDto
                {
                    ClaimType = c.Type,
                    ClaimValue = c.Value
                }).ToList();

                return Ok(BaseResponse<List<PermissionDto>>.Success(permissionDtos, $"Permissions for role '{roleName}' retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving permissions for role {RoleName}", roleName);
                return StatusCode(500, BaseResponse<List<PermissionDto>>.Failure($"An error occurred while retrieving permissions for role '{roleName}'"));
            }
        }

        [HttpPut("{roleName}/permissions")]
        public async Task<ActionResult<BaseResponse<List<PermissionDto>>>> UpdateRolePermissions(string roleName, [FromBody] UpdateRolePermissionsDto updateDto)
        {
            try
            {
                var role = await _identityService.GetRoleByNameAsync(roleName);
                if (role == null)
                {
                    return NotFound(BaseResponse<List<PermissionDto>>.Failure($"Role '{roleName}' not found"));
                }

                var claims = updateDto.Permissions.Select(p => new Claim(p.ClaimType, p.ClaimValue)).ToList();
                var result = await _identityService.UpdateRolePermissionsAsync(roleName, claims);

                if (!result)
                {
                    return BadRequest(BaseResponse<List<PermissionDto>>.Failure($"Failed to update permissions for role '{roleName}'"));
                }

                return Ok(BaseResponse<List<PermissionDto>>.Success(updateDto.Permissions, $"Permissions for role '{roleName}' updated successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating permissions for role {RoleName}", roleName);
                return StatusCode(500, BaseResponse<List<PermissionDto>>.Failure($"An error occurred while updating permissions for role '{roleName}'"));
            }
        }
    }
}
