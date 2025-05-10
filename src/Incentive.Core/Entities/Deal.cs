using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Incentive.Core.Common;
using Incentive.Core.Enums;

namespace Incentive.Core.Entities
{
    /// <summary>
    /// Represents a deal or sale in the system, with customer details, financials and assignment information
    /// </summary>
    public class Deal : SoftDeletableEntity
    {
        [Required]
        [StringLength(200)]
        public string DealName { get; set; } = string.Empty;

        // Customer Information
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

        // Financial Information
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal PaidAmount { get; set; } = 0;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal RemainingAmount { get; set; } // This can be computed, but stored for query efficiency

        [Required]
        [StringLength(50)]
        public string CurrencyType { get; set; } = string.Empty; // Currency used for this deal

        [Column(TypeName = "decimal(5, 2)")]
        public decimal TaxPercentage { get; set; } = 0;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TaxAmount { get; set; } = 0;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal DiscountAmount { get; set; } = 0;

        // Deal Status and Dates
        [Required]
        public DealStatus Status { get; set; }

        [Required]
        public DateTime DealDate { get; set; } // Date when the deal was created

        public DateTime? ClosedDate { get; set; } // Date when the deal was closed (won or lost)

        public DateTime? PaymentDueDate { get; set; } // When payment is due

        // Who closed the deal
        public string? ClosedByUserId { get; set; }

        // Team attribution
        public Guid? TeamId { get; set; }

        // Referral Information
        [StringLength(200)]
        public string? ReferralName { get; set; }

        [StringLength(100)]
        public string? ReferralEmail { get; set; }

        [StringLength(50)]
        public string? ReferralPhone { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? ReferralCommission { get; set; }

        public bool IsReferralCommissionPaid { get; set; } = false;

        // Source and Attribution
        [Required]
        [StringLength(100)]
        public string Source { get; set; } = string.Empty; // How the lead was generated (e.g., Website, Referral, Cold Call)

        // Associated incentive rule
        public Guid? IncentiveRuleId { get; set; }

        [ForeignKey("IncentiveRuleId")]
        public virtual IncentiveRule? AssignedRule { get; set; }

        // Notes and additional context
        [StringLength(2000)]
        public string? Notes { get; set; }

        public int? RecurringFrequencyMonths { get; set; }

        // User relationship (who created/owns the deal)
        public string? UserId { get; set; }

        // Payment tracking
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

        // Deal activity history
        public virtual ICollection<DealActivity> Activities { get; set; } = new List<DealActivity>();

        // Incentive earnings
        public virtual ICollection<IncentiveEarning> IncentiveEarnings { get; set; } = new List<IncentiveEarning>();
    }
}
