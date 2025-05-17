using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Incentive.Application.DTOs
{
    public class TeamDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string TenantId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public string LastModifiedBy { get; set; }
        public List<TeamMemberDto> Members { get; set; } = new List<TeamMemberDto>();
    }

    public class CreateTeamDto
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class UpdateTeamDto
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public bool IsActive { get; set; }
    }

    public class TeamMemberDto
    {
        public Guid Id { get; set; }
        public Guid TeamId { get; set; }
        public string TeamName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime JoinedDate { get; set; }
        public DateTime? LeftDate { get; set; }
    }

    public class AddTeamMemberDto
    {
        [Required]
        public Guid TeamId { get; set; }

        [Required]
        [StringLength(450)]
        public string UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string Role { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class UpdateTeamMemberDto
    {
        [Required]
        [StringLength(100)]
        public string Role { get; set; }

        public bool IsActive { get; set; }

        public DateTime? LeftDate { get; set; }
    }
}
