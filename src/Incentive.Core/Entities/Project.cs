using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Incentive.Core.Common;

namespace Incentive.Core.Entities
{
    /// <summary>
    /// Represents a real estate project in the system
    /// </summary>
    [Schema("IncentiveManagement")]
    public class Project : MultiTenantEntity
    {
        /// <summary>
        /// Short descriptive name for the property/project
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Brief summary of the property/project
        /// </summary>
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Full address or location of the property
        /// </summary>
        [StringLength(255)]
        public string Location { get; set; } = string.Empty;

        /// <summary>
        /// Type of property (apartment, villa, plot, etc.)
        /// </summary>
        [StringLength(50)]
        public string PropertyType { get; set; } = string.Empty;

        /// <summary>
        /// Listing or asking price
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Total built-up or carpet area in square feet
        /// </summary>
        public decimal Area { get; set; }

        /// <summary>
        /// Number of bedrooms
        /// </summary>
        public int Bedrooms { get; set; }

        /// <summary>
        /// Number of bathrooms
        /// </summary>
        public int Bathrooms { get; set; }

        /// <summary>
        /// Date when the property was listed
        /// </summary>
        public DateTime DateListed { get; set; }

        /// <summary>
        /// Current availability (for sale, sold, etc.)
        /// </summary>
        [StringLength(50)]
        public string Status { get; set; } = "For Sale";

        /// <summary>
        /// Name of the listing agent
        /// </summary>
        [StringLength(100)]
        public string AgentName { get; set; } = string.Empty;

        /// <summary>
        /// Contact details of the agent or agency
        /// </summary>
        [StringLength(200)]
        public string AgentContact { get; set; } = string.Empty;

        /// <summary>
        /// Links to property photos or virtual tours (comma-separated)
        /// </summary>
        [StringLength(1000)]
        public string ImagesMedia { get; set; } = string.Empty;

        /// <summary>
        /// List of features/amenities (comma-separated)
        /// </summary>
        [StringLength(500)]
        public string Amenities { get; set; } = string.Empty;

        /// <summary>
        /// Construction year of the property
        /// </summary>
        public int? YearBuilt { get; set; }

        /// <summary>
        /// Ownership type (freehold, leasehold, etc.)
        /// </summary>
        [StringLength(50)]
        public string OwnershipDetails { get; set; } = string.Empty;

        /// <summary>
        /// Date when the listing will expire
        /// </summary>
        public DateTime? ListingExpiryDate { get; set; }

        /// <summary>
        /// Unique property listing identifier
        /// </summary>
        [StringLength(50)]
        public string MLSListingId { get; set; } = string.Empty;


        /// <summary>
        /// Total value of the project
        /// </summary>
        public decimal TotalValue { get; set; }

        /// <summary>
        /// Indicates if the project is active
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}
