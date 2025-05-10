using System;
using Incentive.Core.Entities;

namespace Incentive.Application.DTOs
{
    public class BookingDto
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Guid SalespersonId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public DateTime BookingDate { get; set; }
        public decimal BookingValue { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        
        // Related data
        public string ProjectName { get; set; }
        public string SalespersonName { get; set; }
        public IncentiveEarningDto IncentiveEarning { get; set; }
    }

    public class CreateBookingDto
    {
        public Guid ProjectId { get; set; }
        public Guid SalespersonId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public DateTime BookingDate { get; set; }
        public decimal BookingValue { get; set; }
        public string Notes { get; set; }
    }

    public class UpdateBookingDto
    {
        public Guid ProjectId { get; set; }
        public Guid SalespersonId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public DateTime BookingDate { get; set; }
        public decimal BookingValue { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
    }
}
