using System.Threading.Tasks;
using Incentive.Core.Entities;

namespace Incentive.Application.Interfaces
{
    public interface IIdentityService
    {
        Task<ApplicationUser> GetUserByIdAsync(string userId);
    }
}
