using System;
using System.Threading.Tasks;
using Incentive.Application.Common.Models;
using Incentive.Domain.Entities;
using Incentive.Ports.Repositories;
using Incentive.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Incentive.WebAPI.Controllers
{
    [Authorize]
    public class IncentiveRuleController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public IncentiveRuleController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponse<PaginatedList<IncentiveRule>>>> GetIncentiveRules([FromQuery] PaginationRequest request)
        {
            var query = _unitOfWork.Repository<IncentiveRule>().AsQueryable()
                .Where(r => !r.IsDeleted);

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                query = query.Where(r => 
                    r.Name.Contains(request.SearchTerm) || 
                    r.Description.Contains(request.SearchTerm));
            }

            var paginatedList = await PaginatedList<IncentiveRule>.CreateAsync(
                query, request.PageNumber, request.PageSize);

            return Ok(BaseResponse<PaginatedList<IncentiveRule>>.Success(paginatedList));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse<IncentiveRule>>> GetIncentiveRule(Guid id)
        {
            var incentiveRule = await _unitOfWork.Repository<IncentiveRule>().GetByIdAsync(id);
            if (incentiveRule == null || incentiveRule.IsDeleted)
            {
                return NotFound(BaseResponse<IncentiveRule>.Failure($"Incentive rule with ID {id} not found"));
            }

            return Ok(BaseResponse<IncentiveRule>.Success(incentiveRule));
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse<IncentiveRule>>> CreateIncentiveRule(CreateIncentiveRuleRequest request)
        {
            if (request.ProjectId.HasValue)
            {
                var project = await _unitOfWork.Repository<Project>().GetByIdAsync(request.ProjectId.Value);
                if (project == null || project.IsDeleted)
                {
                    return BadRequest(BaseResponse<IncentiveRule>.Failure($"Project with ID {request.ProjectId} not found"));
                }
            }

            var incentiveRule = new IncentiveRule
            {
                Name = request.Name,
                Description = request.Description,
                ProjectId = request.ProjectId,
                Type = request.Type,
                Value = request.Value,
                MinBookingValue = request.MinBookingValue,
                MaxBookingValue = request.MaxBookingValue,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                IsActive = true
            };

            await _unitOfWork.Repository<IncentiveRule>().AddAsync(incentiveRule);
            await _unitOfWork.SaveChangesAsync();

            return CreatedAtAction(nameof(GetIncentiveRule), new { id = incentiveRule.Id }, BaseResponse<IncentiveRule>.Success(incentiveRule));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BaseResponse<IncentiveRule>>> UpdateIncentiveRule(Guid id, UpdateIncentiveRuleRequest request)
        {
            var incentiveRule = await _unitOfWork.Repository<IncentiveRule>().GetByIdAsync(id);
            if (incentiveRule == null || incentiveRule.IsDeleted)
            {
                return NotFound(BaseResponse<IncentiveRule>.Failure($"Incentive rule with ID {id} not found"));
            }

            if (request.ProjectId.HasValue)
            {
                var project = await _unitOfWork.Repository<Project>().GetByIdAsync(request.ProjectId.Value);
                if (project == null || project.IsDeleted)
                {
                    return BadRequest(BaseResponse<IncentiveRule>.Failure($"Project with ID {request.ProjectId} not found"));
                }
            }

            incentiveRule.Name = request.Name;
            incentiveRule.Description = request.Description;
            incentiveRule.ProjectId = request.ProjectId;
            incentiveRule.Type = request.Type;
            incentiveRule.Value = request.Value;
            incentiveRule.MinBookingValue = request.MinBookingValue;
            incentiveRule.MaxBookingValue = request.MaxBookingValue;
            incentiveRule.StartDate = request.StartDate;
            incentiveRule.EndDate = request.EndDate;
            incentiveRule.IsActive = request.IsActive;

            await _unitOfWork.Repository<IncentiveRule>().UpdateAsync(incentiveRule);
            await _unitOfWork.SaveChangesAsync();

            return Ok(BaseResponse<IncentiveRule>.Success(incentiveRule));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BaseResponse<object>>> DeleteIncentiveRule(Guid id)
        {
            var incentiveRule = await _unitOfWork.Repository<IncentiveRule>().GetByIdAsync(id);
            if (incentiveRule == null || incentiveRule.IsDeleted)
            {
                return NotFound(BaseResponse<object>.Failure($"Incentive rule with ID {id} not found"));
            }

            await _unitOfWork.Repository<IncentiveRule>().DeleteAsync(incentiveRule);
            await _unitOfWork.SaveChangesAsync();

            return Ok(BaseResponse<object>.Success(null, "Incentive rule deleted successfully"));
        }
    }
}
