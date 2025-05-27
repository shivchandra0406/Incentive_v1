using System.Threading.Tasks;
using System.Collections.Generic;
using Incentive.Application.DTOs;
using Incentive.Core.Entities;

namespace Incentive.Application.Interfaces
{
    public interface IIdentityService
    {
        Task<ApplicationUser> GetUserByIdAsync(string userId);

        /// <summary>
        /// Gets a list of users with minimal data (ID and Name only)
        /// </summary>
        /// <returns>List of users with minimal data</returns>
        Task<IEnumerable<UserMinimalDto>> GetUsersMinimalAsync();
    }
}
