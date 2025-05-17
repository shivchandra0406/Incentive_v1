using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Incentive.Application.DTOs;
using Incentive.Core.Entities.IncentivePlan;
using Incentive.Core.Enums;
using Incentive.Application.Interfaces;
using Incentive.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Incentive.Application.Services
{
    public class IncentivePlanService : IIncentivePlanService
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ITenantService _tenantService;
        private readonly ILogger<IncentivePlanService> _logger;

        public IncentivePlanService(
            AppDbContext dbContext,
            IMapper mapper,
            ITenantService tenantService,
            ILogger<IncentivePlanService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _tenantService = tenantService;
            _logger = logger;
        }

        #region General Methods
        public async Task<List<IncentivePlanBaseDto>> GetAllIncentivePlansAsync()
        {
            // Use explicit column selection to avoid referencing columns that might not exist
            var plans = await _dbContext.IncentivePlans
                .AsNoTracking()
                .Where(p => !p.IsDeleted)
                .Select(p => new
                {
                    p.Id,
                    p.PlanName,
                    p.PlanType,
                    p.PeriodType,
                    p.StartDate,
                    p.EndDate,
                    p.IsActive,
                    p.TenantId,
                    p.CreatedBy,
                    p.CreatedAt,
                    p.LastModifiedBy,
                    p.LastModifiedAt,
                    p.IsDeleted,
                    p.DeletedBy,
                    p.DeletedAt,
                    p.PlanDiscriminator
                })
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            // Convert anonymous type to IncentivePlanBaseDto manually
            var result = plans.Select(p => new IncentivePlanBaseDto
            {
                Id = p.Id,
                PlanName = p.PlanName,
                PlanType = p.PlanType,
                PeriodType = p.PeriodType,
                IsActive = p.IsActive,
                TenantId = p.TenantId,
                CreatedBy = p.CreatedBy.ToString(),
                CreatedAt = p.CreatedAt,
                LastModifiedBy = p.LastModifiedBy.ToString(),
                LastModifiedAt = p.LastModifiedAt
            }).ToList();

            return result;
        }

        public async Task<List<IncentivePlanBaseDto>> GetPaginatedIncentivePlansAsync(int page, int limit)
        {
            // Use explicit column selection to avoid referencing columns that might not exist
            var query = _dbContext.IncentivePlans
                .AsNoTracking()
                .Where(p => !p.IsDeleted)
                .Select(p => new
                {
                    p.Id,
                    p.PlanName,
                    p.PlanType,
                    p.PeriodType,
                    p.StartDate,
                    p.EndDate,
                    p.IsActive,
                    p.TenantId,
                    p.CreatedBy,
                    p.CreatedAt,
                    p.LastModifiedBy,
                    p.LastModifiedAt,
                    p.IsDeleted,
                    p.DeletedBy,
                    p.DeletedAt,
                    p.PlanDiscriminator
                })
                .OrderByDescending(p => p.CreatedAt);

            var totalCount = await query.CountAsync();
            var plans = await query
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

            // Convert anonymous type to IncentivePlanBaseDto manually
            var result = plans.Select(p => new IncentivePlanBaseDto
            {
                Id = p.Id,
                PlanName = p.PlanName,
                PlanType = p.PlanType,
                PeriodType = p.PeriodType,
                IsActive = p.IsActive,
                TenantId = p.TenantId,
                CreatedBy = p.CreatedBy.ToString(),
                CreatedAt = p.CreatedAt,
                LastModifiedBy = p.LastModifiedBy.ToString(),
                LastModifiedAt = p.LastModifiedAt
            }).ToList();

            return result;
        }

        public async Task<object> GetIncentivePlanByIdAsync(Guid id)
        {
            var plan = await _dbContext.IncentivePlans
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted); // Global filter applies here

            if (plan == null)
                return null;

            switch (plan.PlanType)
            {
                case IncentivePlanType.TargetBased:
                    var targetPlan = await _dbContext.TargetBasedIncentivePlans
                        .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
                    return _mapper.Map<TargetBasedIncentivePlanDto>(targetPlan);

                case IncentivePlanType.RoleBased:
                    var rolePlan = await _dbContext.RoleBasedIncentivePlans
                        .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
                    return _mapper.Map<RoleBasedIncentivePlanDto>(rolePlan);

                case IncentivePlanType.ProjectBased:
                    var projectPlan = await _dbContext.ProjectBasedIncentivePlans
                        .Include(p => p.Project)
                        .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
                    return _mapper.Map<ProjectBasedIncentivePlanDto>(projectPlan);

                case IncentivePlanType.KickerBased:
                    var kickerPlan = await _dbContext.KickerIncentivePlans
                        .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
                    return _mapper.Map<KickerIncentivePlanDto>(kickerPlan);

                case IncentivePlanType.TieredBased:
                    var tieredPlan = await _dbContext.TieredIncentivePlans
                        .Include(p => p.Tiers.Where(t => !t.IsDeleted)) // ? Corrected here
                        .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
                    return _mapper.Map<TieredIncentivePlanDto>(tieredPlan);

                default:
                    return _mapper.Map<IncentivePlanBaseDto>(plan);
            }
        }
        public async Task<List<object>> GetIncentivePlansByTypeAsync(IncentivePlanType planType)
        {
            switch (planType)
            {
                case IncentivePlanType.TargetBased:
                    var targetPlans = await _dbContext.TargetBasedIncentivePlans.ToListAsync();
                    return _mapper.Map<List<TargetBasedIncentivePlanDto>>(targetPlans).Cast<object>().ToList();

                case IncentivePlanType.RoleBased:
                    var rolePlans = await _dbContext.RoleBasedIncentivePlans.ToListAsync();
                    return _mapper.Map<List<RoleBasedIncentivePlanDto>>(rolePlans).Cast<object>().ToList();

                case IncentivePlanType.ProjectBased:
                    var projectPlans = await _dbContext.ProjectBasedIncentivePlans
                        .Include(p => p.Project)
                        .ToListAsync();
                    return _mapper.Map<List<ProjectBasedIncentivePlanDto>>(projectPlans).Cast<object>().ToList();

                case IncentivePlanType.KickerBased:
                    var kickerPlans = await _dbContext.KickerIncentivePlans.ToListAsync();
                    return _mapper.Map<List<KickerIncentivePlanDto>>(kickerPlans).Cast<object>().ToList();

                case IncentivePlanType.TieredBased:
                    var tieredPlans = await _dbContext.TieredIncentivePlans
                        .Include(p => p.Tiers)
                        .ToListAsync();
                    return _mapper.Map<List<TieredIncentivePlanDto>>(tieredPlans).Cast<object>().ToList();

                default:
                    var plans = await _dbContext.IncentivePlans
                        .Where(p => p.PlanType == planType)
                        .ToListAsync();
                    return _mapper.Map<List<IncentivePlanBaseDto>>(plans).Cast<object>().ToList();
            }
        }

        public async Task<bool> DeleteIncentivePlanAsync(Guid id)
        {
            var plan = await _dbContext.IncentivePlans.FindAsync(id);
            if (plan == null)
            {
                return false;
            }

            _dbContext.IncentivePlans.Remove(plan);
            await _dbContext.SaveChangesAsync();

            return true;
        }
        #endregion

        #region Target-Based Plan Methods
        public async Task<TargetBasedIncentivePlanDto> CreateTargetBasedPlanAsync(CreateTargetBasedIncentivePlanDto createDto)
        {
            var tenantId = _tenantService.GetCurrentTenantId();

            var plan = _mapper.Map<TargetBasedIncentivePlan>(createDto);
            plan.TenantId = tenantId;
            plan.PlanType = IncentivePlanType.TargetBased;
            plan.PlanDiscriminator = "TargetBased";

            _dbContext.TargetBasedIncentivePlans.Add(plan);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<TargetBasedIncentivePlanDto>(plan);
        }

        public async Task<TargetBasedIncentivePlanDto> UpdateTargetBasedPlanAsync(Guid id, UpdateTargetBasedIncentivePlanDto updateDto)
        {
            var plan = await _dbContext.TargetBasedIncentivePlans.FindAsync(id);
            if (plan == null)
            {
                return null;
            }

            _mapper.Map(updateDto, plan);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<TargetBasedIncentivePlanDto>(plan);
        }
        #endregion

        #region Role-Based Plan Methods
        public async Task<RoleBasedIncentivePlanDto> CreateRoleBasedPlanAsync(CreateRoleBasedIncentivePlanDto createDto)
        {
            var tenantId = _tenantService.GetCurrentTenantId();

            var plan = _mapper.Map<RoleBasedIncentivePlan>(createDto);
            plan.TenantId = tenantId;
            plan.PlanType = IncentivePlanType.RoleBased;
            plan.PlanDiscriminator = "RoleBased";

            _dbContext.RoleBasedIncentivePlans.Add(plan);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<RoleBasedIncentivePlanDto>(plan);
        }

        public async Task<RoleBasedIncentivePlanDto> UpdateRoleBasedPlanAsync(Guid id, UpdateRoleBasedIncentivePlanDto updateDto)
        {
            var plan = await _dbContext.RoleBasedIncentivePlans.FindAsync(id);
            if (plan == null)
            {
                return null;
            }

            _mapper.Map(updateDto, plan);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<RoleBasedIncentivePlanDto>(plan);
        }
        #endregion

        #region Project-Based Plan Methods
        public async Task<ProjectBasedIncentivePlanDto> CreateProjectBasedPlanAsync(CreateProjectBasedIncentivePlanDto createDto)
        {
            var tenantId = _tenantService.GetCurrentTenantId();

            var plan = _mapper.Map<ProjectBasedIncentivePlan>(createDto);
            plan.TenantId = tenantId;
            plan.PlanType = IncentivePlanType.ProjectBased;
            plan.PlanDiscriminator = "ProjectBased";

            _dbContext.ProjectBasedIncentivePlans.Add(plan);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<ProjectBasedIncentivePlanDto>(plan);
        }

        public async Task<ProjectBasedIncentivePlanDto> UpdateProjectBasedPlanAsync(Guid id, UpdateProjectBasedIncentivePlanDto updateDto)
        {
            var plan = await _dbContext.ProjectBasedIncentivePlans.FindAsync(id);
            if (plan == null)
            {
                return null;
            }

            _mapper.Map(updateDto, plan);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<ProjectBasedIncentivePlanDto>(plan);
        }
        #endregion

        #region Kicker-Based Plan Methods
        public async Task<KickerIncentivePlanDto> CreateKickerBasedPlanAsync(CreateKickerIncentivePlanDto createDto)
        {
            var tenantId = _tenantService.GetCurrentTenantId();

            var plan = _mapper.Map<KickerIncentivePlan>(createDto);
            plan.TenantId = tenantId;
            plan.PlanType = IncentivePlanType.KickerBased;
            plan.PlanDiscriminator = "KickerBased";

            _dbContext.KickerIncentivePlans.Add(plan);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<KickerIncentivePlanDto>(plan);
        }

        public async Task<KickerIncentivePlanDto> UpdateKickerBasedPlanAsync(Guid id, UpdateKickerIncentivePlanDto updateDto)
        {
            var plan = await _dbContext.KickerIncentivePlans.FindAsync(id);
            if (plan == null)
            {
                return null;
            }

            _mapper.Map(updateDto, plan);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<KickerIncentivePlanDto>(plan);
        }
        #endregion

        #region Tiered-Based Plan Methods
        public async Task<TieredIncentivePlanDto> CreateTieredBasedPlanAsync(CreateTieredIncentivePlanDto createDto)
        {
            var tenantId = _tenantService.GetCurrentTenantId();

            var plan = _mapper.Map<TieredIncentivePlan>(createDto);
            plan.TenantId = tenantId;
            plan.PlanType = IncentivePlanType.TieredBased;
            plan.PlanDiscriminator = "TieredBased";

            _dbContext.TieredIncentivePlans.Add(plan);
            await _dbContext.SaveChangesAsync();

            // Add tiers
            foreach (var tierDto in createDto.Tiers)
            {
                var tier = _mapper.Map<TieredIncentiveTier>(tierDto);
                tier.TieredIncentivePlanId = plan.Id;
                tier.TenantId = tenantId;

                _dbContext.TieredIncentiveTiers.Add(tier);
            }

            await _dbContext.SaveChangesAsync();

            // Fetch the complete plan with tiers
            var createdPlan = await _dbContext.TieredIncentivePlans
                .Include(p => p.Tiers)
                .FirstOrDefaultAsync(p => p.Id == plan.Id);

            return _mapper.Map<TieredIncentivePlanDto>(createdPlan);
        }

        public async Task<TieredIncentivePlanDto> UpdateTieredBasedPlanAsync(Guid id, UpdateTieredIncentivePlanDto updateDto)
        {
            var plan = await _dbContext.TieredIncentivePlans
                .Include(p => p.Tiers.Where(x => !x.IsDeleted))
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (plan == null)
                return null;

            // Remove old tiers
            _dbContext.TieredIncentiveTiers.RemoveRange(plan.Tiers);
            await _dbContext.SaveChangesAsync();
            // Map updated fields of plan (but be cautious with concurrency fields)
            _mapper.Map(updateDto, plan);

            // Add new tiers
            var tenantId = _tenantService.GetCurrentTenantId();
            foreach (var tierDto in updateDto.Tiers)
            {
                var tier = _mapper.Map<TieredIncentiveTier>(tierDto);
                tier.TieredIncentivePlanId = plan.Id;
                tier.TenantId = tenantId;
                _dbContext.TieredIncentiveTiers.Add(tier);
            }

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception("The incentive plan was updated by another user. Please reload and try again.", ex);
            }

            var updatedPlan = await _dbContext.TieredIncentivePlans
                .Include(p => p.Tiers)
                .FirstOrDefaultAsync(p => p.Id == id);

            return _mapper.Map<TieredIncentivePlanDto>(updatedPlan);
        }
        #endregion
    }
}
