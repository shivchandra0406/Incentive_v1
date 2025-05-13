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
    [Route("api/users")]
    [Authorize(Policy = "RequireAdminRole")]
    [RequiresTenantId(description: "The tenant ID to access user data")]
    public class UserPermissionsController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly ITenantService _tenantService;
        private readonly ILogger<UserPermissionsController> _logger;

        public UserPermissionsController(
            IIdentityService identityService,
            ITenantService tenantService,
            ILogger<UserPermissionsController> logger)
        {
            _identityService = identityService;
            _tenantService = tenantService;
            _logger = logger;
        }
        [AllowAnonymous]
        [HttpGet("default-permission")]
        public async Task<ActionResult<BaseResponse<List<string>>>> GetDefaultPermission()
        {
            return Ok(BaseResponse<List<string>>.Success(_identityService.GetDefaultPermission(), "User permissions retrieved successfully"));
        }

        [HttpGet("{userId}/permissions")]
        public async Task<ActionResult<BaseResponse<UserPermissionsDto>>> GetUserPermissions(string userId)
        {
            try
            {
                var user = await _identityService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(BaseResponse<UserPermissionsDto>.Failure($"User with ID '{userId}' not found"));
                }

                var (roles, rolePermissions, directPermissions) = await _identityService.GetUserPermissionsAsync(userId);

                // Convert to DTOs
                var rolePermissionDtos = new Dictionary<string, List<PermissionDto>>();
                foreach (var role in rolePermissions)
                {
                    rolePermissionDtos[role.Key] = role.Value.Select(c => new PermissionDto
                    {
                        ClaimType = c.Type,
                        ClaimValue = c.Value
                    }).ToList();
                }

                var directPermissionDtos = directPermissions.Select(c => new PermissionDto
                {
                    ClaimType = c.Type,
                    ClaimValue = c.Value
                }).ToList();

                // Calculate effective permissions (combine all permissions from roles and direct permissions)
                var effectivePermissions = new List<PermissionDto>();
                var addedPermissions = new HashSet<string>();

                // Add role permissions
                foreach (var rolePermission in rolePermissionDtos.Values)
                {
                    foreach (var permission in rolePermission)
                    {
                        var key = $"{permission.ClaimType}:{permission.ClaimValue}";
                        if (!addedPermissions.Contains(key))
                        {
                            effectivePermissions.Add(permission);
                            addedPermissions.Add(key);
                        }
                    }
                }

                // Add direct permissions
                foreach (var permission in directPermissionDtos)
                {
                    var key = $"{permission.ClaimType}:{permission.ClaimValue}";
                    if (!addedPermissions.Contains(key))
                    {
                        effectivePermissions.Add(permission);
                        addedPermissions.Add(key);
                    }
                }

                var result = new UserPermissionsDto
                {
                    UserId = userId,
                    UserName = user.UserName,
                    Roles = roles.ToList(),
                    RolePermissions = rolePermissionDtos,
                    DirectPermissions = directPermissionDtos,
                    EffectivePermissions = effectivePermissions
                };

                return Ok(BaseResponse<UserPermissionsDto>.Success(result, "User permissions retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving permissions for user {UserId}", userId);
                return StatusCode(500, BaseResponse<UserPermissionsDto>.Failure($"An error occurred while retrieving permissions for user '{userId}'"));
            }
        }

        [HttpPost("{userId}/direct-permissions")]
        public async Task<ActionResult<BaseResponse<UserPermissionsDto>>> AssignPermissionsToUser(string userId, [FromBody] AssignUserPermissionsDto assignDto)
        {
            try
            {
                var user = await _identityService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(BaseResponse<UserPermissionsDto>.Failure($"User with ID '{userId}' not found"));
                }

                // Get current user claims
                var currentClaims = await _identityService.GetUserClaimsAsync(userId);

                // Add new permission claims
                foreach (var permission in assignDto.Permissions)
                {
                    // Check if claim already exists
                    if (!currentClaims.Any(c => c.Type == permission.ClaimType && c.Value == permission.ClaimValue))
                    {
                        var result = await _identityService.AddClaimToUserAsync(userId, permission.ClaimType, permission.ClaimValue);
                        if (!result)
                        {
                            return BadRequest(BaseResponse<UserPermissionsDto>.Failure($"Failed to add permission '{permission.ClaimValue}' to user"));
                        }
                    }
                }

                // Return updated permissions
                return await GetUserPermissions(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning permissions to user {UserId}", userId);
                return StatusCode(500, BaseResponse<UserPermissionsDto>.Failure($"An error occurred while assigning permissions to user '{userId}'"));
            }
        }

        [HttpPut("{userId}/direct-permissions")]
        public async Task<ActionResult<BaseResponse<UserPermissionsDto>>> UpdateUserDirectPermissions(string userId, [FromBody] UpdateUserPermissionsDto updateDto)
        {
            try
            {
                var user = await _identityService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(BaseResponse<UserPermissionsDto>.Failure($"User with ID '{userId}' not found"));
                }

                // Get current user claims
                var currentClaims = await _identityService.GetUserClaimsAsync(userId);

                // Filter out permission claims
                var currentPermissionClaims = currentClaims.Where(c => c.Type == "Permission").ToList();
                var otherClaims = currentClaims.Where(c => c.Type != "Permission").ToList();

                // Create new claims list with non-permission claims and new permission claims
                var newClaims = new List<Claim>(otherClaims);
                newClaims.AddRange(updateDto.Permissions.Select(p => new Claim(p.ClaimType, p.ClaimValue)));

                // Update user claims
                var result = await _identityService.UpdateUserClaimsAsync(userId, newClaims);
                if (!result)
                {
                    return BadRequest(BaseResponse<UserPermissionsDto>.Failure($"Failed to update permissions for user '{userId}'"));
                }

                // Return updated permissions
                return await GetUserPermissions(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating permissions for user {UserId}", userId);
                return StatusCode(500, BaseResponse<UserPermissionsDto>.Failure($"An error occurred while updating permissions for user '{userId}'"));
            }
        }

        [HttpPost("{userId}/direct-permissions/add")]
        public async Task<ActionResult<BaseResponse<UserPermissionsDto>>> AddUserDirectPermission(string userId, [FromBody] PermissionDto permissionDto)
        {
            try
            {
                var user = await _identityService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(BaseResponse<UserPermissionsDto>.Failure($"User with ID '{userId}' not found"));
                }

                var result = await _identityService.AddClaimToUserAsync(userId, permissionDto.ClaimType, permissionDto.ClaimValue);
                if (!result)
                {
                    return BadRequest(BaseResponse<UserPermissionsDto>.Failure($"Failed to add permission '{permissionDto.ClaimValue}' to user"));
                }

                // Return updated permissions
                return await GetUserPermissions(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding permission to user {UserId}", userId);
                return StatusCode(500, BaseResponse<UserPermissionsDto>.Failure($"An error occurred while adding permission to user '{userId}'"));
            }
        }

        [HttpPost("{userId}/direct-permissions/remove")]
        public async Task<ActionResult<BaseResponse<UserPermissionsDto>>> RemoveUserDirectPermission(string userId, [FromBody] PermissionDto permissionDto)
        {
            try
            {
                var user = await _identityService.GetUserByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(BaseResponse<UserPermissionsDto>.Failure($"User with ID '{userId}' not found"));
                }

                var result = await _identityService.RemoveClaimFromUserAsync(userId, permissionDto.ClaimType, permissionDto.ClaimValue);
                if (!result)
                {
                    return BadRequest(BaseResponse<UserPermissionsDto>.Failure($"Failed to remove permission '{permissionDto.ClaimValue}' from user"));
                }

                // Return updated permissions
                return await GetUserPermissions(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing permission from user {UserId}", userId);
                return StatusCode(500, BaseResponse<UserPermissionsDto>.Failure($"An error occurred while removing permission from user '{userId}'"));
            }
        }
    }
}
