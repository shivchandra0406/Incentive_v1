using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Incentive.Core.Entities;
using Incentive.Core.Enums;

namespace Incentive.Core.Interfaces
{
    public interface IDealActivityRepository : IRepository<DealActivity>
    {
        Task<IReadOnlyList<DealActivity>> GetActivitiesByDealIdAsync(Guid dealId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<DealActivity>> GetActivitiesByTypeAsync(ActivityType type, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<DealActivity>> GetActivitiesByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<DealActivity>> GetActivitiesByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
        Task SoftDeleteAsync(DealActivity entity, CancellationToken cancellationToken = default);
    }
}
