using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Incentive.Core.Entities;

namespace Incentive.Core.Interfaces
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        Task<IReadOnlyList<Payment>> GetPaymentsByDealIdAsync(Guid dealId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Payment>> GetPaymentsByMethodAsync(string paymentMethod, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Payment>> GetPaymentsByReceivedByUserIdAsync(string userId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Payment>> GetUnverifiedPaymentsAsync(CancellationToken cancellationToken = default);
        Task<decimal> GetTotalPaymentsAmountAsync(CancellationToken cancellationToken = default);
        Task<decimal> GetTotalPaymentsAmountByDealIdAsync(Guid dealId, CancellationToken cancellationToken = default);
        Task SoftDeleteAsync(Payment entity, CancellationToken cancellationToken = default);
    }
}
