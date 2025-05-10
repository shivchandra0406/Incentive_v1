using System.Security.Claims;
using Incentive.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Incentive.Infrastructure.Identity
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }

    public interface ICurrentUserService
    {
        string GetUserId();
    }
}
