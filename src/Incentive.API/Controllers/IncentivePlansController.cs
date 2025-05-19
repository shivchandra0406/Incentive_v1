using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Incentive.API.Attributes;
using Incentive.Application.Common.Models;
using Incentive.Application.DTOs;
using Incentive.Core.Enums;
using Incentive.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Incentive.API.Controllers
{
    [ApiController]
    [Route("api/incentive-plans")]
    [Authorize]
    [RequiresTenantId(description: "The tenant ID to access incentive plan data")]
    public class IncentivePlansController : ControllerBase
    {
        private readonly IIncentivePlanService _incentivePlanService;
        private readonly ILogger<IncentivePlansController> _logger;

        public IncentivePlansController(IIncentivePlanService incentivePlanService, ILogger<IncentivePlansController> logger)
        {
            _incentivePlanService = incentivePlanService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponse<List<IncentivePlanBaseDto>>>> GetAllIncentivePlans([FromQuery] int page = 1, [FromQuery] int limit = 10)
        {
            try
            {
                var plans = await _incentivePlanService.GetPaginatedIncentivePlansAsync(page, limit);
                return Ok(BaseResponse<List<IncentivePlanBaseDto>>.Success(plans, "Incentive plans retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving incentive plans");
                return StatusCode(500, BaseResponse<List<IncentivePlanBaseDto>>.Failure("An error occurred while retrieving incentive plans"));
            }
        }

        /// <summary>
        /// Get minimal incentive plan data (ID and Name only)
        /// </summary>
        /// <returns>List of incentive plans with minimal data</returns>
        [HttpGet("minimal")]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<IncentivePlanMinimalDto>>), 200)]
        public async Task<ActionResult<BaseResponse<IEnumerable<IncentivePlanMinimalDto>>>> GetIncentivePlansMinimal()
        {
            try
            {
                var plans = await _incentivePlanService.GetIncentivePlansMinimalAsync();
                return Ok(BaseResponse<IEnumerable<IncentivePlanMinimalDto>>.Success(plans, "Minimal incentive plan data retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving minimal incentive plan data");
                return StatusCode(500, BaseResponse<IEnumerable<IncentivePlanMinimalDto>>.Failure("An error occurred while retrieving minimal incentive plan data"));
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse<object>>> GetIncentivePlanById(Guid id)
        {
            try
            {
                var plan = await _incentivePlanService.GetIncentivePlanByIdAsync(id);
                if (plan == null)
                {
                    return NotFound(BaseResponse<object>.Failure($"Incentive plan with ID {id} not found"));
                }

                return Ok(BaseResponse<object>.Success(plan, "Incentive plan retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving incentive plan with ID {PlanId}", id);
                return StatusCode(500, BaseResponse<object>.Failure($"An error occurred while retrieving incentive plan with ID {id}"));
            }
        }

        [HttpGet("types/{planType}")]
        public async Task<ActionResult<BaseResponse<List<object>>>> GetIncentivePlansByType(IncentivePlanType planType)
        {
            try
            {
                var plans = await _incentivePlanService.GetIncentivePlansByTypeAsync(planType);
                return Ok(BaseResponse<List<object>>.Success(plans, $"{planType} incentive plans retrieved successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving incentive plans of type {PlanType}", planType);
                return StatusCode(500, BaseResponse<List<object>>.Failure($"An error occurred while retrieving incentive plans of type {planType}"));
            }
        }

        #region Target-Based Plans
        [HttpPost("target-based")]
        public async Task<ActionResult<BaseResponse<TargetBasedIncentivePlanDto>>> CreateTargetBasedPlan([FromBody] CreateTargetBasedIncentivePlanDto createDto)
        {
            try
            {
                var plan = await _incentivePlanService.CreateTargetBasedPlanAsync(createDto);
                return CreatedAtAction(nameof(GetIncentivePlanById), new { id = plan.Id }, BaseResponse<TargetBasedIncentivePlanDto>.Success(plan, "Target-based incentive plan created successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating target-based incentive plan");
                return StatusCode(500, BaseResponse<TargetBasedIncentivePlanDto>.Failure("An error occurred while creating the target-based incentive plan"));
            }
        }

        [HttpPut("target-based/{id}")]
        public async Task<ActionResult<BaseResponse<TargetBasedIncentivePlanDto>>> UpdateTargetBasedPlan(Guid id, [FromBody] UpdateTargetBasedIncentivePlanDto updateDto)
        {
            try
            {
                var plan = await _incentivePlanService.UpdateTargetBasedPlanAsync(id, updateDto);
                if (plan == null)
                {
                    return NotFound(BaseResponse<TargetBasedIncentivePlanDto>.Failure($"Target-based incentive plan with ID {id} not found"));
                }

                return Ok(BaseResponse<TargetBasedIncentivePlanDto>.Success(plan, "Target-based incentive plan updated successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating target-based incentive plan with ID {PlanId}", id);
                return StatusCode(500, BaseResponse<TargetBasedIncentivePlanDto>.Failure($"An error occurred while updating target-based incentive plan with ID {id}"));
            }
        }
        #endregion

        #region Role-Based Plans
        [HttpPost("role-based")]
        public async Task<ActionResult<BaseResponse<RoleBasedIncentivePlanDto>>> CreateRoleBasedPlan([FromBody] CreateRoleBasedIncentivePlanDto createDto)
        {
            try
            {
                var plan = await _incentivePlanService.CreateRoleBasedPlanAsync(createDto);
                return CreatedAtAction(nameof(GetIncentivePlanById), new { id = plan.Id }, BaseResponse<RoleBasedIncentivePlanDto>.Success(plan, "Role-based incentive plan created successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating role-based incentive plan");
                return StatusCode(500, BaseResponse<RoleBasedIncentivePlanDto>.Failure("An error occurred while creating the role-based incentive plan"));
            }
        }

        [HttpPut("role-based/{id}")]
        public async Task<ActionResult<BaseResponse<RoleBasedIncentivePlanDto>>> UpdateRoleBasedPlan(Guid id, [FromBody] UpdateRoleBasedIncentivePlanDto updateDto)
        {
            try
            {
                var plan = await _incentivePlanService.UpdateRoleBasedPlanAsync(id, updateDto);
                if (plan == null)
                {
                    return NotFound(BaseResponse<RoleBasedIncentivePlanDto>.Failure($"Role-based incentive plan with ID {id} not found"));
                }

                return Ok(BaseResponse<RoleBasedIncentivePlanDto>.Success(plan, "Role-based incentive plan updated successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating role-based incentive plan with ID {PlanId}", id);
                return StatusCode(500, BaseResponse<RoleBasedIncentivePlanDto>.Failure($"An error occurred while updating role-based incentive plan with ID {id}"));
            }
        }
        #endregion

        #region Project-Based Plans
        [HttpPost("project-based")]
        public async Task<ActionResult<BaseResponse<ProjectBasedIncentivePlanDto>>> CreateProjectBasedPlan([FromBody] CreateProjectBasedIncentivePlanDto createDto)
        {
            try
            {
                var plan = await _incentivePlanService.CreateProjectBasedPlanAsync(createDto);
                return CreatedAtAction(nameof(GetIncentivePlanById), new { id = plan.Id }, BaseResponse<ProjectBasedIncentivePlanDto>.Success(plan, "Project-based incentive plan created successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating project-based incentive plan");
                return StatusCode(500, BaseResponse<ProjectBasedIncentivePlanDto>.Failure("An error occurred while creating the project-based incentive plan"));
            }
        }

        [HttpPut("project-based/{id}")]
        public async Task<ActionResult<BaseResponse<ProjectBasedIncentivePlanDto>>> UpdateProjectBasedPlan(Guid id, [FromBody] UpdateProjectBasedIncentivePlanDto updateDto)
        {
            try
            {
                var plan = await _incentivePlanService.UpdateProjectBasedPlanAsync(id, updateDto);
                if (plan == null)
                {
                    return NotFound(BaseResponse<ProjectBasedIncentivePlanDto>.Failure($"Project-based incentive plan with ID {id} not found"));
                }

                return Ok(BaseResponse<ProjectBasedIncentivePlanDto>.Success(plan, "Project-based incentive plan updated successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating project-based incentive plan with ID {PlanId}", id);
                return StatusCode(500, BaseResponse<ProjectBasedIncentivePlanDto>.Failure($"An error occurred while updating project-based incentive plan with ID {id}"));
            }
        }
        #endregion

        #region Kicker-Based Plans
        [HttpPost("kicker-based")]
        public async Task<ActionResult<BaseResponse<KickerIncentivePlanDto>>> CreateKickerBasedPlan([FromBody] CreateKickerIncentivePlanDto createDto)
        {
            try
            {
                var plan = await _incentivePlanService.CreateKickerBasedPlanAsync(createDto);
                return CreatedAtAction(nameof(GetIncentivePlanById), new { id = plan.Id }, BaseResponse<KickerIncentivePlanDto>.Success(plan, "Kicker-based incentive plan created successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating kicker-based incentive plan");
                return StatusCode(500, BaseResponse<KickerIncentivePlanDto>.Failure("An error occurred while creating the kicker-based incentive plan"));
            }
        }

        [HttpPut("kicker-based/{id}")]
        public async Task<ActionResult<BaseResponse<KickerIncentivePlanDto>>> UpdateKickerBasedPlan(Guid id, [FromBody] UpdateKickerIncentivePlanDto updateDto)
        {
            try
            {
                var plan = await _incentivePlanService.UpdateKickerBasedPlanAsync(id, updateDto);
                if (plan == null)
                {
                    return NotFound(BaseResponse<KickerIncentivePlanDto>.Failure($"Kicker-based incentive plan with ID {id} not found"));
                }

                return Ok(BaseResponse<KickerIncentivePlanDto>.Success(plan, "Kicker-based incentive plan updated successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating kicker-based incentive plan with ID {PlanId}", id);
                return StatusCode(500, BaseResponse<KickerIncentivePlanDto>.Failure($"An error occurred while updating kicker-based incentive plan with ID {id}"));
            }
        }
        #endregion

        #region Tiered-Based Plans
        [HttpPost("tiered-based")]
        public async Task<ActionResult<BaseResponse<TieredIncentivePlanDto>>> CreateTieredBasedPlan([FromBody] CreateTieredIncentivePlanDto createDto)
        {
            try
            {
                var plan = await _incentivePlanService.CreateTieredBasedPlanAsync(createDto);
                return CreatedAtAction(nameof(GetIncentivePlanById), new { id = plan.Id }, BaseResponse<TieredIncentivePlanDto>.Success(plan, "Tiered-based incentive plan created successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating tiered-based incentive plan");
                return StatusCode(500, BaseResponse<TieredIncentivePlanDto>.Failure("An error occurred while creating the tiered-based incentive plan"));
            }
        }

        [HttpPut("tiered-based/{id}")]
        public async Task<ActionResult<BaseResponse<TieredIncentivePlanDto>>> UpdateTieredBasedPlan(Guid id, [FromBody] UpdateTieredIncentivePlanDto updateDto)
        {
            try
            {
                var plan = await _incentivePlanService.UpdateTieredBasedPlanAsync(id, updateDto);
                if (plan == null)
                {
                    return NotFound(BaseResponse<TieredIncentivePlanDto>.Failure($"Tiered-based incentive plan with ID {id} not found"));
                }

                return Ok(BaseResponse<TieredIncentivePlanDto>.Success(plan, "Tiered-based incentive plan updated successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating tiered-based incentive plan with ID {PlanId}", id);
                return StatusCode(500, BaseResponse<TieredIncentivePlanDto>.Failure($"An error occurred while updating tiered-based incentive plan with ID {id}"));
            }
        }
        #endregion

        [HttpDelete("{id}")]
        public async Task<ActionResult<BaseResponse<bool>>> DeleteIncentivePlan(Guid id)
        {
            try
            {
                var result = await _incentivePlanService.DeleteIncentivePlanAsync(id);
                if (!result)
                {
                    return NotFound(BaseResponse<bool>.Failure($"Incentive plan with ID {id} not found"));
                }

                return Ok(BaseResponse<bool>.Success(true, "Incentive plan deleted successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting incentive plan with ID {PlanId}", id);
                return StatusCode(500, BaseResponse<bool>.Failure($"An error occurred while deleting incentive plan with ID {id}"));
            }
        }
    }
}
