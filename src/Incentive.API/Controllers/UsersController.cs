using Incentive.API.Attributes;
using Incentive.Application.Common.Models;
using Incentive.Application.DTOs;
using Incentive.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Incentive.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "RequireAdminRole")]
    [RequiresTenantId(description: "The tenant ID to access user data")]
    public class UsersController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly ITenantService _tenantService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(
            IIdentityService identityService,
            ITenantService tenantService,
            ILogger<UsersController> logger)
        {
            _identityService = identityService;
            _tenantService = tenantService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponse<IEnumerable<UserDto>>>> GetAllUsers()
        {
            try
            {
                var tenantId = _tenantService.GetCurrentTenantId();
                var users = await _identityService.GetUsersByTenantIdAsync(tenantId);
                var userDtos = new List<UserDto>();

                foreach (var user in users)
                {
                    var roles = await _identityService.GetUserRolesAsync(user.Id);
                    var claims = (await _identityService.GetRoleClaimsAsync(roles.FirstOrDefault() ?? "")).Select(c => new RoleClaimDto
                    {
                        ClaimType = c.Type,
                        ClaimValue = c.Value
                    }).ToList();

                    userDtos.Add(new UserDto
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        TenantId = user.TenantId,
                        IsActive = user.IsActive,
                        CreatedAt = user.CreatedAt,
                        CreatedBy = user.CreatedBy,
                        LastModifiedAt = user.LastModifiedAt,
                        LastModifiedBy = user.LastModifiedBy,
                        Roles = roles.ToList(),
                        Claims = claims
                    });
                }

                return Ok(BaseResponse<IEnumerable<UserDto>>.Success(userDtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving users");
                return StatusCode(500, BaseResponse<IEnumerable<UserDto>>.Failure("An error occurred while retrieving users"));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse<UserDto>>> GetUser(string id)
        {
            try
            {
                var user = await _identityService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound(BaseResponse<UserDto>.Failure("User not found"));
                }

                var tenantId = _tenantService.GetCurrentTenantId();
                if (user.TenantId != tenantId)
                {
                    return Forbid();
                }

                var roles = await _identityService.GetUserRolesAsync(user.Id);
                var claims = (await _identityService.GetRoleClaimsAsync(roles.FirstOrDefault() ?? "")).Select(c => new RoleClaimDto
                {
                    ClaimType = c.Type,
                    ClaimValue = c.Value
                }).ToList();

                var userDto = new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    TenantId = user.TenantId,
                    IsActive = user.IsActive,
                    CreatedAt = user.CreatedAt,
                    CreatedBy = user.CreatedBy,
                    LastModifiedAt = user.LastModifiedAt,
                    LastModifiedBy = user.LastModifiedBy,
                    Roles = roles.ToList(),
                    Claims = claims
                };

                return Ok(BaseResponse<UserDto>.Success(userDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user with ID {UserId}", id);
                return StatusCode(500, BaseResponse<UserDto>.Failure("An error occurred while retrieving the user"));
            }
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse<UserResponseDto>>> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            try
            {
                if (createUserDto.Password != createUserDto.ConfirmPassword)
                {
                    return BadRequest(BaseResponse<UserResponseDto>.Failure("Passwords do not match"));
                }

                var tenantId = _tenantService.GetCurrentTenantId();
                if (string.IsNullOrEmpty(createUserDto.TenantId))
                {
                    createUserDto.TenantId = tenantId;
                }
                else if (createUserDto.TenantId != tenantId)
                {
                    return BadRequest(BaseResponse<UserResponseDto>.Failure("Cannot create user for a different tenant"));
                }

                var result = await _identityService.CreateUserWithRolesAsync(
                    createUserDto.UserName,
                    createUserDto.Email,
                    createUserDto.Password,
                    createUserDto.FirstName,
                    createUserDto.LastName,
                    createUserDto.TenantId,
                    createUserDto.Roles.Count > 0 ? createUserDto.Roles : new List<string> { "Admin" });

                if (!result.Succeeded)
                {
                    return BadRequest(BaseResponse<UserResponseDto>.Failure(result.Message));
                }

                var response = new UserResponseDto
                {
                    Succeeded = true,
                    UserId = result.UserId,
                    Message = "User created successfully"
                };

                return CreatedAtAction(nameof(GetUser), new { id = result.UserId }, BaseResponse<UserResponseDto>.Success(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return StatusCode(500, BaseResponse<UserResponseDto>.Failure("An error occurred while creating the user"));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BaseResponse<UserResponseDto>>> UpdateUser(string id, [FromBody] UpdateUserDto updateUserDto)
        {
            try
            {
                var user = await _identityService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound(BaseResponse<UserResponseDto>.Failure("User not found"));
                }

                var tenantId = _tenantService.GetCurrentTenantId();
                if (user.TenantId != tenantId)
                {
                    return Forbid();
                }

                var result = await _identityService.UpdateUserAsync(
                    id,
                    updateUserDto.Email,
                    updateUserDto.FirstName,
                    updateUserDto.LastName,
                    updateUserDto.IsActive);

                if (!result.Succeeded)
                {
                    return BadRequest(BaseResponse<UserResponseDto>.Failure(result.Message));
                }

                var response = new UserResponseDto
                {
                    Succeeded = true,
                    UserId = id,
                    Message = "User updated successfully"
                };

                return Ok(BaseResponse<UserResponseDto>.Success(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user with ID {UserId}", id);
                return StatusCode(500, BaseResponse<UserResponseDto>.Failure("An error occurred while updating the user"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BaseResponse<UserResponseDto>>> DeleteUser(string id)
        {
            try
            {
                var user = await _identityService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound(BaseResponse<UserResponseDto>.Failure("User not found"));
                }

                var tenantId = _tenantService.GetCurrentTenantId();
                if (user.TenantId != tenantId)
                {
                    return Forbid();
                }

                var result = await _identityService.DeleteUserAsync(id);
                if (!result)
                {
                    return BadRequest(BaseResponse<UserResponseDto>.Failure("Failed to delete user"));
                }

                var response = new UserResponseDto
                {
                    Succeeded = true,
                    UserId = id,
                    Message = "User deleted successfully"
                };

                return Ok(BaseResponse<UserResponseDto>.Success(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user with ID {UserId}", id);
                return StatusCode(500, BaseResponse<UserResponseDto>.Failure("An error occurred while deleting the user"));
            }
        }

        [HttpPost("{id}/roles")]
        public async Task<ActionResult<BaseResponse<UserResponseDto>>> AssignRoles(string id, [FromBody] UserRolesDto userRolesDto)
        {
            try
            {
                if (id != userRolesDto.UserId)
                {
                    return BadRequest(BaseResponse<UserResponseDto>.Failure("User ID mismatch"));
                }

                var user = await _identityService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound(BaseResponse<UserResponseDto>.Failure("User not found"));
                }

                var tenantId = _tenantService.GetCurrentTenantId();
                if (user.TenantId != tenantId)
                {
                    return Forbid();
                }

                // Get current roles
                var currentRoles = await _identityService.GetUserRolesAsync(id);

                // Remove roles that are not in the new list
                foreach (var role in currentRoles)
                {
                    if (!userRolesDto.Roles.Contains(role))
                    {
                        await _identityService.RemoveUserFromRoleAsync(id, role);
                    }
                }

                // Add new roles
                foreach (var role in userRolesDto.Roles)
                {
                    if (!currentRoles.Contains(role))
                    {
                        await _identityService.AddUserToRoleAsync(id, role);
                    }
                }

                var response = new UserResponseDto
                {
                    Succeeded = true,
                    UserId = id,
                    Message = "User roles updated successfully"
                };

                return Ok(BaseResponse<UserResponseDto>.Success(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning roles to user with ID {UserId}", id);
                return StatusCode(500, BaseResponse<UserResponseDto>.Failure("An error occurred while assigning roles to the user"));
            }
        }

        [HttpGet("{id}/roles")]
        public async Task<ActionResult<BaseResponse<IEnumerable<string>>>> GetUserRoles(string id)
        {
            try
            {
                var user = await _identityService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound(BaseResponse<IEnumerable<string>>.Failure("User not found"));
                }

                var tenantId = _tenantService.GetCurrentTenantId();
                if (user.TenantId != tenantId)
                {
                    return Forbid();
                }

                var roles = await _identityService.GetUserRolesAsync(id);
                return Ok(BaseResponse<IEnumerable<string>>.Success(roles));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving roles for user with ID {UserId}", id);
                return StatusCode(500, BaseResponse<IEnumerable<string>>.Failure("An error occurred while retrieving user roles"));
            }
        }

        [HttpPost("change-password")]
        public async Task<ActionResult<BaseResponse<UserResponseDto>>> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            try
            {
                if (changePasswordDto.NewPassword != changePasswordDto.ConfirmNewPassword)
                {
                    return BadRequest(BaseResponse<UserResponseDto>.Failure("New passwords do not match"));
                }

                var user = await _identityService.GetUserByIdAsync(changePasswordDto.UserId);
                if (user == null)
                {
                    return NotFound(BaseResponse<UserResponseDto>.Failure("User not found"));
                }

                var tenantId = _tenantService.GetCurrentTenantId();
                if (user.TenantId != tenantId)
                {
                    return Forbid();
                }

                var result = await _identityService.ChangePasswordAsync(
                    changePasswordDto.UserId,
                    changePasswordDto.CurrentPassword,
                    changePasswordDto.NewPassword);

                if (!result.Succeeded)
                {
                    return BadRequest(BaseResponse<UserResponseDto>.Failure(result.Message));
                }

                var response = new UserResponseDto
                {
                    Succeeded = true,
                    UserId = changePasswordDto.UserId,
                    Message = "Password changed successfully"
                };

                return Ok(BaseResponse<UserResponseDto>.Success(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing password for user with ID {UserId}", changePasswordDto.UserId);
                return StatusCode(500, BaseResponse<UserResponseDto>.Failure("An error occurred while changing the password"));
            }
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult<BaseResponse<UserResponseDto>>> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            try
            {
                if (resetPasswordDto.NewPassword != resetPasswordDto.ConfirmNewPassword)
                {
                    return BadRequest(BaseResponse<UserResponseDto>.Failure("New passwords do not match"));
                }

                var user = await _identityService.GetUserByIdAsync(resetPasswordDto.UserId);
                if (user == null)
                {
                    return NotFound(BaseResponse<UserResponseDto>.Failure("User not found"));
                }

                var tenantId = _tenantService.GetCurrentTenantId();
                if (user.TenantId != tenantId)
                {
                    return Forbid();
                }

                var result = await _identityService.ResetPasswordAsync(
                    resetPasswordDto.UserId,
                    resetPasswordDto.NewPassword);

                if (!result.Succeeded)
                {
                    return BadRequest(BaseResponse<UserResponseDto>.Failure(result.Message));
                }

                var response = new UserResponseDto
                {
                    Succeeded = true,
                    UserId = resetPasswordDto.UserId,
                    Message = "Password reset successfully"
                };

                return Ok(BaseResponse<UserResponseDto>.Success(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting password for user with ID {UserId}", resetPasswordDto.UserId);
                return StatusCode(500, BaseResponse<UserResponseDto>.Failure("An error occurred while resetting the password"));
            }
        }

        [HttpGet("{id}/claims")]
        public async Task<ActionResult<BaseResponse<IEnumerable<RoleClaimDto>>>> GetUserClaims(string id)
        {
            try
            {
                var user = await _identityService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound(BaseResponse<IEnumerable<RoleClaimDto>>.Failure("User not found"));
                }

                var tenantId = _tenantService.GetCurrentTenantId();
                if (user.TenantId != tenantId)
                {
                    return Forbid();
                }

                var claims = await _identityService.GetUserClaimsAsync(id);
                var claimDtos = claims.Select(c => new RoleClaimDto
                {
                    ClaimType = c.Type,
                    ClaimValue = c.Value
                }).ToList();

                return Ok(BaseResponse<IEnumerable<RoleClaimDto>>.Success(claimDtos));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving claims for user with ID {UserId}", id);
                return StatusCode(500, BaseResponse<IEnumerable<RoleClaimDto>>.Failure("An error occurred while retrieving user claims"));
            }
        }

        [HttpPost("{id}/claims")]
        public async Task<ActionResult<BaseResponse<UserResponseDto>>> UpdateUserClaims(string id, [FromBody] UserClaimsDto userClaimsDto)
        {
            try
            {
                if (id != userClaimsDto.UserId)
                {
                    return BadRequest(BaseResponse<UserResponseDto>.Failure("User ID mismatch"));
                }

                var user = await _identityService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound(BaseResponse<UserResponseDto>.Failure("User not found"));
                }

                var tenantId = _tenantService.GetCurrentTenantId();
                if (user.TenantId != tenantId)
                {
                    return Forbid();
                }

                var claims = userClaimsDto.Claims.Select(c => new System.Security.Claims.Claim(c.ClaimType, c.ClaimValue)).ToList();
                var result = await _identityService.UpdateUserClaimsAsync(id, claims);

                if (!result)
                {
                    return BadRequest(BaseResponse<UserResponseDto>.Failure("Failed to update user claims"));
                }

                var response = new UserResponseDto
                {
                    Succeeded = true,
                    UserId = id,
                    Message = "User claims updated successfully"
                };

                return Ok(BaseResponse<UserResponseDto>.Success(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating claims for user with ID {UserId}", id);
                return StatusCode(500, BaseResponse<UserResponseDto>.Failure("An error occurred while updating user claims"));
            }
        }

        [HttpPost("{id}/claims/add")]
        public async Task<ActionResult<BaseResponse<UserResponseDto>>> AddUserClaim(string id, [FromBody] RoleClaimDto claimDto)
        {
            try
            {
                var user = await _identityService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound(BaseResponse<UserResponseDto>.Failure("User not found"));
                }

                var tenantId = _tenantService.GetCurrentTenantId();
                if (user.TenantId != tenantId)
                {
                    return Forbid();
                }

                var result = await _identityService.AddClaimToUserAsync(id, claimDto.ClaimType, claimDto.ClaimValue);

                if (!result)
                {
                    return BadRequest(BaseResponse<UserResponseDto>.Failure("Failed to add claim to user"));
                }

                var response = new UserResponseDto
                {
                    Succeeded = true,
                    UserId = id,
                    Message = "Claim added to user successfully"
                };

                return Ok(BaseResponse<UserResponseDto>.Success(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding claim to user with ID {UserId}", id);
                return StatusCode(500, BaseResponse<UserResponseDto>.Failure("An error occurred while adding claim to user"));
            }
        }

        [HttpPost("{id}/claims/remove")]
        public async Task<ActionResult<BaseResponse<UserResponseDto>>> RemoveUserClaim(string id, [FromBody] RoleClaimDto claimDto)
        {
            try
            {
                var user = await _identityService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound(BaseResponse<UserResponseDto>.Failure("User not found"));
                }

                var tenantId = _tenantService.GetCurrentTenantId();
                if (user.TenantId != tenantId)
                {
                    return Forbid();
                }

                var result = await _identityService.RemoveClaimFromUserAsync(id, claimDto.ClaimType, claimDto.ClaimValue);

                if (!result)
                {
                    return BadRequest(BaseResponse<UserResponseDto>.Failure("Failed to remove claim from user"));
                }

                var response = new UserResponseDto
                {
                    Succeeded = true,
                    UserId = id,
                    Message = "Claim removed from user successfully"
                };

                return Ok(BaseResponse<UserResponseDto>.Success(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing claim from user with ID {UserId}", id);
                return StatusCode(500, BaseResponse<UserResponseDto>.Failure("An error occurred while removing claim from user"));
            }
        }
    }
}
