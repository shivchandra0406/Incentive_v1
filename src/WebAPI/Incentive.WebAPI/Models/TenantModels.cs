using System.ComponentModel.DataAnnotations;

namespace Incentive.WebAPI.Models
{
    public class CreateTenantRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Identifier { get; set; }

        [Required]
        public string ConnectionString { get; set; }

        [Required]
        public string AdminUserName { get; set; }

        [Required]
        [EmailAddress]
        public string AdminEmail { get; set; }

        [Required]
        [MinLength(8)]
        public string AdminPassword { get; set; }

        [Required]
        public string AdminFirstName { get; set; }

        [Required]
        public string AdminLastName { get; set; }
    }

    public class UpdateTenantRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string ConnectionString { get; set; }
    }
}
