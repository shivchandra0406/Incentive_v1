using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Incentive.Core.Common;

namespace Incentive.Core.Entities
{
    /// <summary>
    /// Payments made against deals
    /// </summary>
    [Schema("IncentiveManagement")]
    public class Payment : MultiTenantEntity
    {
        public Guid DealId { get; set; }

        [ForeignKey("DealId")]
        public virtual Deal Deal { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; } = string.Empty; // e.g., Cash, Credit Card, Bank Transfer

        [StringLength(100)]
        public string? TransactionReference { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        public string? ReceivedByUserId { get; set; }

        public bool IsVerified { get; set; } = false;
    }
}
