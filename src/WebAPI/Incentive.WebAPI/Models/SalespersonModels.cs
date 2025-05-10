using System;
using System.ComponentModel.DataAnnotations;

namespace Incentive.WebAPI.Models
{
    public class CreateSalespersonRequest
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        public string EmployeeId { get; set; }

        [Required]
        public DateTime HireDate { get; set; }
    }

    public class UpdateSalespersonRequest
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        public string EmployeeId { get; set; }

        [Required]
        public DateTime HireDate { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
