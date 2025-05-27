using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Incentive.Application.DTOs;
using Incentive.Core.Enums;

namespace Incentive.Application.Interfaces
{
    public interface IIncentivePlanService
    {
        // General methods
        Task<List<IncentivePlanBaseDto>> GetAllIncentivePlansAsync();
        Task<List<IncentivePlanBaseDto>> GetPaginatedIncentivePlansAsync(int page, int limit);
        Task<object> GetIncentivePlanByIdAsync(Guid id);
        Task<List<object>> GetIncentivePlansByTypeAsync(IncentivePlanType planType);
        Task<bool> DeleteIncentivePlanAsync(Guid id);

        /// <summary>
        /// Gets a list of incentive plans with minimal data (ID and Name only)
        /// </summary>
        /// <returns>List of incentive plans with minimal data</returns>
        Task<IEnumerable<IncentivePlanMinimalDto>> GetIncentivePlansMinimalAsync();

        // Target-based plan methods
        Task<TargetBasedIncentivePlanDto> CreateTargetBasedPlanAsync(CreateTargetBasedIncentivePlanDto createDto);
        Task<TargetBasedIncentivePlanDto> UpdateTargetBasedPlanAsync(Guid id, UpdateTargetBasedIncentivePlanDto updateDto);

        // Role-based plan methods
        Task<RoleBasedIncentivePlanDto> CreateRoleBasedPlanAsync(CreateRoleBasedIncentivePlanDto createDto);
        Task<RoleBasedIncentivePlanDto> UpdateRoleBasedPlanAsync(Guid id, UpdateRoleBasedIncentivePlanDto updateDto);

        // Project-based plan methods
        Task<ProjectBasedIncentivePlanDto> CreateProjectBasedPlanAsync(CreateProjectBasedIncentivePlanDto createDto);
        Task<ProjectBasedIncentivePlanDto> UpdateProjectBasedPlanAsync(Guid id, UpdateProjectBasedIncentivePlanDto updateDto);

        // Kicker-based plan methods
        Task<KickerIncentivePlanDto> CreateKickerBasedPlanAsync(CreateKickerIncentivePlanDto createDto);
        Task<KickerIncentivePlanDto> UpdateKickerBasedPlanAsync(Guid id, UpdateKickerIncentivePlanDto updateDto);

        // Tiered-based plan methods
        Task<TieredIncentivePlanDto> CreateTieredBasedPlanAsync(CreateTieredIncentivePlanDto createDto);
        Task<TieredIncentivePlanDto> UpdateTieredBasedPlanAsync(Guid id, UpdateTieredIncentivePlanDto updateDto);
    }
}
