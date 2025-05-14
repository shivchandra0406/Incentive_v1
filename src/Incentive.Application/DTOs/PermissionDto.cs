using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Incentive.Application.DTOs
{
    public class PermissionDto
    {
        public string ClaimType { get; set; } = "Permission";
        public string ClaimValue { get; set; } = string.Empty;
    }

    public class UserPermissionsDto
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new List<string>();
        public Dictionary<string, List<PermissionDto>> RolePermissions { get; set; } = new Dictionary<string, List<PermissionDto>>();
        public List<PermissionDto> DirectPermissions { get; set; } = new List<PermissionDto>();
        public List<PermissionDto> EffectivePermissions { get; set; } = new List<PermissionDto>();
    }

    public class RolePermissionsDto
    {
        public string RoleName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<PermissionDto> Permissions { get; set; } = new List<PermissionDto>();
    }

    public class UpdateRolePermissionsDto
    {
        [Required]
        public List<PermissionDto> Permissions { get; set; } = new List<PermissionDto>();
    }

    public class AssignUserPermissionsDto
    {
        [Required]
        public List<PermissionDto> Permissions { get; set; } = new List<PermissionDto>();
    }

    public class UpdateUserPermissionsDto
    {
        [Required]
        public List<PermissionDto> Permissions { get; set; } = new List<PermissionDto>();
    }
}
