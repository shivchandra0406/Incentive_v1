using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Incentive.Application.DTOs
{
    /// <summary>
    /// Minimal user data containing only ID and Name
    /// </summary>
    public class UserMinimalDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class UserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TenantId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public string LastModifiedBy { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
        public List<RoleClaimDto> Claims { get; set; } = new List<RoleClaimDto>();
    }

    public class CreateUserDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string TenantId { get; set; }

        public bool IsActive { get; set; } = true;

        public List<string> Roles { get; set; } = new List<string>();
    }

    public class UpdateUserDto
    {
        [EmailAddress]
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool? IsActive { get; set; }
    }

    public class ChangePasswordDto
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        [MinLength(8)]
        public string CurrentPassword { get; set; }

        [Required]
        [MinLength(8)]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }
    }

    public class ResetPasswordDto
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        [MinLength(8)]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; }
    }

    public class UserRolesDto
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public List<string> Roles { get; set; } = new List<string>();
    }

    public class UserClaimsDto
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public List<RoleClaimDto> Claims { get; set; } = new List<RoleClaimDto>();
    }

    public class UserResponseDto
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public string UserId { get; set; }
    }
}
