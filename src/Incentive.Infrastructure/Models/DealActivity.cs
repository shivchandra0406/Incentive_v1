using System;
using System.Collections.Generic;

namespace Incentive.Infrastructure.Models;

public partial class DealActivity
{
    public Guid Id { get; set; }

    public Guid DealId { get; set; }

    public string Type { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? Notes { get; set; }

    public DateTime ActivityDate { get; set; }

    public Guid UserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? LastModifiedAt { get; set; }

    public Guid LastModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public string TenantId { get; set; } = null!;

    public virtual Deal Deal { get; set; } = null!;
}
