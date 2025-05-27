using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Incentive.Core.Entities;
using Incentive.Core.Enums;

namespace Incentive.Core.Interfaces
{
    public interface IDealRepository : IRepository<Deal>
    {
        Task<IReadOnlyList<Deal>> GetDealsByStatusAsync(DealStatus status, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Deal>> GetDealsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Deal>> GetDealsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Deal>> GetDealsByIncentivePlanIdAsync(Guid incentivePlanId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Deal>> GetDealsByCustomerNameAsync(string customerName, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Deal>> GetDealsBySourceAsync(string source, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Deal>> GetDealsWithPendingPaymentsAsync(CancellationToken cancellationToken = default);
        Task<decimal> GetTotalDealAmountAsync(CancellationToken cancellationToken = default);
        Task<decimal> GetTotalPaidAmountAsync(CancellationToken cancellationToken = default);
        Task<decimal> GetTotalRemainingAmountAsync(CancellationToken cancellationToken = default);
        Task SoftDeleteAsync(Deal entity, CancellationToken cancellationToken = default);
    }
}
