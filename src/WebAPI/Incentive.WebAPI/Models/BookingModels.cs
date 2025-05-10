using System;
using System.ComponentModel.DataAnnotations;
using Incentive.Domain.Enums;

namespace Incentive.WebAPI.Models
{
    public class CreateBookingRequest
    {
        [Required]
        public Guid ProjectId { get; set; }

        [Required]
        public Guid SalespersonId { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [EmailAddress]
        public string CustomerEmail { get; set; }

        public string CustomerPhone { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal BookingValue { get; set; }

        [Required]
        public BookingStatus Status { get; set; }

        public string Notes { get; set; }
    }

    public class UpdateBookingRequest
    {
        [Required]
        public Guid ProjectId { get; set; }

        [Required]
        public Guid SalespersonId { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [EmailAddress]
        public string CustomerEmail { get; set; }

        public string CustomerPhone { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal BookingValue { get; set; }

        [Required]
        public BookingStatus Status { get; set; }

        public string Notes { get; set; }
    }
}
