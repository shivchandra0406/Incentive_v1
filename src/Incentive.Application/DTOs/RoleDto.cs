using System;
using System.Collections.Generic;

namespace Incentive.Application.DTOs
{
    public class RoleDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string TenantId { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public string LastModifiedBy { get; set; }
    }

    public class CreateRoleDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string TenantId { get; set; }
    }

    public class UpdateRoleDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class RoleClaimDto
    {
        public int Id { get; set; }
        public string RoleId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }

    public class CreateRoleClaimDto
    {
        public string RoleId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }

    public class UserRoleDto
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
    }

    public class AssignRoleDto
    {
        public string UserId { get; set; }
        public string RoleName { get; set; }
    }
}
