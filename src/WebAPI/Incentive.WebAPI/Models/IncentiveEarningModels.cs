using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Incentive.Domain.Entities;
using Incentive.Domain.Enums;

namespace Incentive.WebAPI.Models
{
    public class UpdateIncentiveEarningStatusRequest
    {
        [Required]
        public IncentiveEarningStatus Status { get; set; }
    }

    public class IncentiveReportRequest
    {
        public Guid? SalespersonId { get; set; }
        public Guid? ProjectId { get; set; }
        public IncentiveEarningStatus? Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class IncentiveReport
    {
        public int TotalEarnings { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PendingAmount { get; set; }
        public decimal ApprovedAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RejectedAmount { get; set; }
        public List<IncentiveEarning> Earnings { get; set; }
    }
}
