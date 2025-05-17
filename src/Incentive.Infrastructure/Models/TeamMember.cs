using System;
using System.Collections.Generic;

namespace Incentive.Infrastructure.Models;

public partial class TeamMember
{
    public Guid Id { get; set; }

    public Guid TeamId { get; set; }

    public string UserId { get; set; } = null!;

    public string Role { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime JoinedDate { get; set; }

    public DateTime? LeftDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? LastModifiedAt { get; set; }

    public Guid LastModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public string TenantId { get; set; } = null!;

    public virtual Team Team { get; set; } = null!;
}
