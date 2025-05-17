using System;
using System.ComponentModel.DataAnnotations;

namespace Incentive.Application.DTOs
{
    /// <summary>
    /// Minimal project data containing only ID and Name
    /// </summary>
    public class ProjectMinimalDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
    public class ProjectDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string PropertyType { get; set; }
        public decimal Price { get; set; }
        public decimal Area { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public DateTime DateListed { get; set; }
        public string Status { get; set; }
        public string AgentName { get; set; }
        public string AgentContact { get; set; }
        public string ImagesMedia { get; set; }
        public string Amenities { get; set; }
        public int? YearBuilt { get; set; }
        public string OwnershipDetails { get; set; }
        public DateTime? ListingExpiryDate { get; set; }
        public string MLSListingId { get; set; }
        public decimal TotalValue { get; set; }
        public bool IsActive { get; set; }
        public string TenantId { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateProjectDto
    {

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(255)]
        public string Location { get; set; }

        [StringLength(50)]
        public string PropertyType { get; set; }
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Area { get; set; }

        [Range(0, int.MaxValue)]
        public int Bedrooms { get; set; }

        [Range(0, int.MaxValue)]
        public int Bathrooms { get; set; }
        public DateTime DateListed { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "For Sale";

        [StringLength(100)]
        public string AgentName { get; set; }

        [StringLength(200)]
        public string AgentContact { get; set; }

        [StringLength(1000)]
        public string ImagesMedia { get; set; }

        [StringLength(500)]
        public string Amenities { get; set; }

        public int? YearBuilt { get; set; }

        [StringLength(50)]
        public string OwnershipDetails { get; set; }

        public DateTime? ListingExpiryDate { get; set; }

        [StringLength(50)]
        public string MLSListingId { get; set; }
        public decimal TotalValue { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class UpdateProjectDto
    {

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(255)]
        public string Location { get; set; }

        [StringLength(50)]
        public string PropertyType { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Area { get; set; }

        [Range(0, int.MaxValue)]
        public int Bedrooms { get; set; }

        [Range(0, int.MaxValue)]
        public int Bathrooms { get; set; }

        public DateTime DateListed { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(100)]
        public string AgentName { get; set; }

        [StringLength(200)]
        public string AgentContact { get; set; }

        [StringLength(1000)]
        public string ImagesMedia { get; set; }

        [StringLength(500)]
        public string Amenities { get; set; }

        public int? YearBuilt { get; set; }

        [StringLength(50)]
        public string OwnershipDetails { get; set; }

        public DateTime? ListingExpiryDate { get; set; }

        [StringLength(50)]
        public string MLSListingId { get; set; }
        [Range(0, double.MaxValue)]
        public decimal TotalValue { get; set; }
        public bool IsActive { get; set; }
    }
}
