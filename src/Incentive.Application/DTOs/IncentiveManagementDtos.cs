using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Incentive.Core.Enums;

namespace Incentive.Application.DTOs
{
    #region IncentiveRule DTOs

    public class IncentiveRuleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public TargetFrequency Frequency { get; set; }
        public AppliedRuleType AppliedTo { get; set; }
        public CurrencyType Currency { get; set; }
        public TargetType Target { get; set; }
        public IncentiveCalculationType Incentive { get; set; }
        public decimal? TargetValue { get; set; }
        public int? TargetDealCount { get; set; }
        public decimal? Commission { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsIncludeSalary { get; set; }
        public decimal? MinimumSalesThreshold { get; set; }
        public int? MinimumDealCountThreshold { get; set; }
        public decimal? MaximumIncentiveAmount { get; set; }
        public string? UserId { get; set; }
        public Guid? TeamId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public string? LastModifiedBy { get; set; }
    }

    public class CreateIncentiveRuleDto
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public TargetFrequency Frequency { get; set; }

        [Required]
        public AppliedRuleType AppliedTo { get; set; }

        [Required]
        public CurrencyType Currency { get; set; }

        [Required]
        public TargetType Target { get; set; }

        [Required]
        public IncentiveCalculationType Incentive { get; set; }

        public decimal? TargetValue { get; set; }

        public int? TargetDealCount { get; set; }

        public decimal? Commission { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsIncludeSalary { get; set; } = true;

        public decimal? MinimumSalesThreshold { get; set; }

        public int? MinimumDealCountThreshold { get; set; }

        public decimal? MaximumIncentiveAmount { get; set; }

        public string? UserId { get; set; }

        public Guid? TeamId { get; set; }
    }

    public class UpdateIncentiveRuleDto
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public TargetFrequency Frequency { get; set; }

        [Required]
        public AppliedRuleType AppliedTo { get; set; }

        [Required]
        public CurrencyType Currency { get; set; }

        [Required]
        public TargetType Target { get; set; }

        [Required]
        public IncentiveCalculationType Incentive { get; set; }

        public decimal? TargetValue { get; set; }

        public int? TargetDealCount { get; set; }

        public decimal? Commission { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; }

        public bool IsIncludeSalary { get; set; }

        public decimal? MinimumSalesThreshold { get; set; }

        public int? MinimumDealCountThreshold { get; set; }

        public decimal? MaximumIncentiveAmount { get; set; }

        public string? UserId { get; set; }

        public Guid? TeamId { get; set; }
    }

    #endregion

    #region Deal DTOs

    /// <summary>
    /// Minimal deal data containing only ID and Name
    /// </summary>
    public class DealMinimalDto
    {
        public Guid Id { get; set; }
        public string DealName { get; set; }
    }

    public class DealDto
    {
        public Guid Id { get; set; }
        public string DealName { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string? CustomerEmail { get; set; }
        public string? CustomerPhone { get; set; }
        public string? CustomerAddress { get; set; }
        public Guid? ClosedByUserId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public decimal TaxPercentage { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public DealStatus Status { get; set; }
        public DateTime DealDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public DateTime? PaymentDueDate { get; set; }
        public Guid? TeamId { get; set; }
        public string? ReferralName { get; set; }
        public string? ReferralEmail { get; set; }
        public string? ReferralPhone { get; set; }
        public decimal? ReferralCommission { get; set; }
        public bool IsReferralCommissionPaid { get; set; }
        public string Source { get; set; } = string.Empty;
        public Guid IncentivePlanId { get; set; }
        public string? Notes { get; set; }
        public int? RecurringFrequencyMonths { get; set; }
        public string? UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public string? LastModifiedBy { get; set; }
        public List<PaymentDto> Payments { get; set; } = new List<PaymentDto>();
        public List<DealActivityDto> Activities { get; set; } = new List<DealActivityDto>();
    }

    public class CreateDealDto
    {
        [Required]
        [StringLength(200)]
        public string DealName { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string CustomerName { get; set; } = string.Empty;

        [StringLength(100)]
        [EmailAddress]
        public string? CustomerEmail { get; set; }

        [StringLength(50)]
        public string? CustomerPhone { get; set; }

        [StringLength(500)]
        public string? CustomerAddress { get; set; }
        [Required]
        public Guid? ClosedByUserId { get; set; }
        
        public decimal PaidAmount { get; set; } = 0;
        public decimal RemainingAmount { get; set; }
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal TotalAmount { get; set; }

        [Required]
        public CurrencyType CurrencyType { get; set; }

        [Range(0, 100)]
        public decimal TaxPercentage { get; set; } = 0;

        [Range(0, double.MaxValue)]
        public decimal DiscountAmount { get; set; } = 0;

        [Required]
        public DealStatus Status { get; set; } = DealStatus.New;

        [Required]
        public DateTime DealDate { get; set; } = DateTime.UtcNow;

        public DateTime? PaymentDueDate { get; set; }

        public Guid? TeamId { get; set; }

        [StringLength(200)]
        public string? ReferralName { get; set; }

        [StringLength(100)]
        [EmailAddress]
        public string? ReferralEmail { get; set; }

        [StringLength(50)]
        public string? ReferralPhone { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? ReferralCommission { get; set; }

        [StringLength(100)]
        public string Source { get; set; } = string.Empty;

        public Guid IncentivePlanId { get; set; }

        [StringLength(2000)]
        public string? Notes { get; set; }

        [Range(0, 60)]
        public int? RecurringFrequencyMonths { get; set; }
    }

    public class UpdateDealDto
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(200)]
        public string DealName { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string CustomerName { get; set; } = string.Empty;

        [StringLength(100)]
        [EmailAddress]
        public string? CustomerEmail { get; set; }

        [StringLength(50)]
        public string? CustomerPhone { get; set; }

        [StringLength(500)]
        public string? CustomerAddress { get; set; }
        [Required]
        public Guid? ClosedByUserId { get; set; }
        public decimal PaidAmount { get; set; } = 0;
        public decimal RemainingAmount { get; set; }
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal TotalAmount { get; set; }

        [Required]
        public CurrencyType CurrencyType { get; set; }

        [Range(0, 100)]
        public decimal TaxPercentage { get; set; }

        [Range(0, double.MaxValue)]
        public decimal DiscountAmount { get; set; }

        [Required]
        public DealStatus Status { get; set; }

        public DateTime? ClosedDate { get; set; }

        public DateTime? PaymentDueDate { get; set; }

        public Guid? TeamId { get; set; }

        [StringLength(200)]
        public string? ReferralName { get; set; }

        [StringLength(100)]
        [EmailAddress]
        public string? ReferralEmail { get; set; }

        [StringLength(50)]
        public string? ReferralPhone { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? ReferralCommission { get; set; }

        public bool IsReferralCommissionPaid { get; set; }

        [StringLength(100)]
        public string Source { get; set; } = string.Empty;

        public Guid IncentivePlanId { get; set; }

        [StringLength(2000)]
        public string? Notes { get; set; }

        [Range(0, 60)]
        public int? RecurringFrequencyMonths { get; set; }
    }

    #endregion

    #region Payment DTOs

    public class PaymentDto
    {
        public Guid Id { get; set; }
        public Guid DealId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string? TransactionReference { get; set; }
        public string? Notes { get; set; }
        public string? ReceivedByUserId { get; set; }
        public bool IsVerified { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? LastModifiedAt { get; set; }
        public string? LastModifiedBy { get; set; }
    }

    public class CreatePaymentDto
    {
        [Required]
        public Guid DealId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; } = string.Empty;

        [StringLength(100)]
        public string? TransactionReference { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }
    }

    public class UpdatePaymentDto
    {
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; } = string.Empty;

        [StringLength(100)]
        public string? TransactionReference { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        public bool IsVerified { get; set; }
    }

    #endregion

    #region DealActivity DTOs

    public class DealActivityDto
    {
        public Guid Id { get; set; }
        public Guid DealId { get; set; }
        public ActivityType Type { get; set; }
        public string Description { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public string? UserId { get; set; }
        public DateTime ActivityDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
    }

    public class CreateDealActivityDto
    {
        [Required]
        public Guid DealId { get; set; }

        [Required]
        public ActivityType Type { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Notes { get; set; }
    }

    public class UpdateDealActivityDto
    {
        [Required]
        public ActivityType Type { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Notes { get; set; }
    }

    #endregion
}
