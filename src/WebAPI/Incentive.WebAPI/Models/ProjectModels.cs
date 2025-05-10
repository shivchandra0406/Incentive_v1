using System;
using System.ComponentModel.DataAnnotations;

namespace Incentive.WebAPI.Models
{
    public class CreateProjectRequest
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal TotalValue { get; set; }
    }

    public class UpdateProjectRequest
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal TotalValue { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
