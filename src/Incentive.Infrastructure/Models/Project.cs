using System;
using System.Collections.Generic;

namespace Incentive.Infrastructure.Models;

public partial class Project
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Location { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public decimal TotalValue { get; set; }

    public bool IsActive { get; set; }

    public Guid UserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? LastModifiedAt { get; set; }

    public Guid LastModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public string TenantId { get; set; } = null!;

    public virtual ICollection<IncentivePlan> IncentivePlans { get; set; } = new List<IncentivePlan>();
}
