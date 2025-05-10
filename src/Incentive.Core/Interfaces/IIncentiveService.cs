using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Incentive.Core.Entities;

namespace Incentive.Core.Interfaces
{
    public interface IIncentiveService
    {
        Task<IncentiveEarning> CalculateIncentiveAsync(Guid bookingId);
        Task<IncentiveEarning> ApproveIncentiveAsync(Guid incentiveEarningId);
        Task<IncentiveEarning> RejectIncentiveAsync(Guid incentiveEarningId, string reason);
        Task<IncentiveEarning> MarkAsPaidAsync(Guid incentiveEarningId);
        Task<IEnumerable<IncentiveEarning>> GetIncentiveEarningsBySalespersonAsync(Guid salespersonId);
        Task<IEnumerable<IncentiveEarning>> GetIncentiveEarningsByProjectAsync(Guid projectId);
        Task<IEnumerable<IncentiveEarning>> GetIncentiveEarningsByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
