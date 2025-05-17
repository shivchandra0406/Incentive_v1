using System;
using System.Collections.Generic;

namespace Incentive.Infrastructure.Models;

public partial class Deal
{
    public Guid Id { get; set; }

    public string DealName { get; set; } = null!;

    public string CustomerName { get; set; } = null!;

    public string? CustomerEmail { get; set; }

    public string? CustomerPhone { get; set; }

    public string? CustomerAddress { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal PaidAmount { get; set; }

    public decimal RemainingAmount { get; set; }

    public string CurrencyType { get; set; } = null!;

    public decimal TaxPercentage { get; set; }

    public decimal TaxAmount { get; set; }

    public decimal DiscountAmount { get; set; }

    public string Status { get; set; } = null!;

    public DateTime DealDate { get; set; }

    public DateTime? ClosedDate { get; set; }

    public DateTime? PaymentDueDate { get; set; }

    public string? ClosedByUserId { get; set; }

    public Guid? TeamId { get; set; }

    public string? ReferralName { get; set; }

    public string? ReferralEmail { get; set; }

    public string? ReferralPhone { get; set; }

    public decimal? ReferralCommission { get; set; }

    public bool IsReferralCommissionPaid { get; set; }

    public string Source { get; set; } = null!;

    public Guid? IncentiveRuleId { get; set; }

    public string? Notes { get; set; }

    public int? RecurringFrequencyMonths { get; set; }

    public string? UserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? LastModifiedAt { get; set; }

    public Guid LastModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Guid? DeletedBy { get; set; }

    public string TenantId { get; set; } = null!;

    public virtual ICollection<DealActivity> DealActivities { get; set; } = new List<DealActivity>();

    public virtual ICollection<IncentiveEarning> IncentiveEarnings { get; set; } = new List<IncentiveEarning>();

    public virtual IncentiveRule? IncentiveRule { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
