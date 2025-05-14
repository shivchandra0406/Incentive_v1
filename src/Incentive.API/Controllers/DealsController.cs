using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Incentive.API.Attributes;
using Incentive.Application.Common.Models;
using Incentive.Application.DTOs;
using Incentive.Core.Entities;
using Incentive.Core.Enums;
using Incentive.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Incentive.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [RequiresTenantId(description: "The tenant ID to access deals data")]
    public class DealsController : ControllerBase
    {
        private readonly IDealRepository _dealRepository;
        private readonly IDealActivityRepository _dealActivityRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DealsController> _logger;

        public DealsController(
            IDealRepository dealRepository,
            IDealActivityRepository dealActivityRepository,
            IMapper mapper,
            ILogger<DealsController> logger)
        {
            _dealRepository = dealRepository;
            _dealActivityRepository = dealActivityRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse<IEnumerable<DealDto>>>> GetAllDeals()
        {
            var deals = await _dealRepository.GetAllAsync();
            var dealDtos = _mapper.Map<IEnumerable<DealDto>>(deals);
            return Ok(BaseResponse<IEnumerable<DealDto>>.Success(dealDtos));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseResponse<DealDto>>> GetDealById(Guid id)
        {
            var deal = await _dealRepository.GetByIdAsync(id);
            if (deal == null)
            {
                return NotFound(BaseResponse<DealDto>.Failure($"Deal with ID {id} not found"));
            }

            var dealDto = _mapper.Map<DealDto>(deal);
            return Ok(BaseResponse<DealDto>.Success(dealDto));
        }

        [HttpGet("status/{status}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse<IEnumerable<DealDto>>>> GetDealsByStatus(DealStatus status)
        {
            var deals = await _dealRepository.GetDealsByStatusAsync(status);
            var dealDtos = _mapper.Map<IEnumerable<DealDto>>(deals);
            return Ok(BaseResponse<IEnumerable<DealDto>>.Success(dealDtos));
        }

        [HttpGet("user/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse<IEnumerable<DealDto>>>> GetDealsByUserId(Guid userId)
        {
            var deals = await _dealRepository.GetDealsByUserIdAsync(userId);
            var dealDtos = _mapper.Map<IEnumerable<DealDto>>(deals);
            return Ok(BaseResponse<IEnumerable<DealDto>>.Success(dealDtos));
        }

        [HttpGet("date-range")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResponse<IEnumerable<DealDto>>>> GetDealsByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate > endDate)
            {
                return BadRequest(BaseResponse<IEnumerable<DealDto>>.Failure("Start date cannot be later than end date"));
            }

            var deals = await _dealRepository.GetDealsByDateRangeAsync(startDate, endDate);
            var dealDtos = _mapper.Map<IEnumerable<DealDto>>(deals);
            return Ok(BaseResponse<IEnumerable<DealDto>>.Success(dealDtos));
        }

        [HttpGet("incentive-rule/{incentiveRuleId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse<IEnumerable<DealDto>>>> GetDealsByIncentiveRuleId(Guid incentiveRuleId)
        {
            var deals = await _dealRepository.GetDealsByIncentiveRuleIdAsync(incentiveRuleId);
            var dealDtos = _mapper.Map<IEnumerable<DealDto>>(deals);
            return Ok(BaseResponse<IEnumerable<DealDto>>.Success(dealDtos));
        }

        [HttpGet("customer/{customerName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse<IEnumerable<DealDto>>>> GetDealsByCustomerName(string customerName)
        {
            var deals = await _dealRepository.GetDealsByCustomerNameAsync(customerName);
            var dealDtos = _mapper.Map<IEnumerable<DealDto>>(deals);
            return Ok(BaseResponse<IEnumerable<DealDto>>.Success(dealDtos));
        }

        [HttpGet("source/{source}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse<IEnumerable<DealDto>>>> GetDealsBySource(string source)
        {
            var deals = await _dealRepository.GetDealsBySourceAsync(source);
            var dealDtos = _mapper.Map<IEnumerable<DealDto>>(deals);
            return Ok(BaseResponse<IEnumerable<DealDto>>.Success(dealDtos));
        }

        [HttpGet("pending-payments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse<IEnumerable<DealDto>>>> GetDealsWithPendingPayments()
        {
            var deals = await _dealRepository.GetDealsWithPendingPaymentsAsync();
            var dealDtos = _mapper.Map<IEnumerable<DealDto>>(deals);
            return Ok(BaseResponse<IEnumerable<DealDto>>.Success(dealDtos));
        }

        [HttpGet("total-amount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse<decimal>>> GetTotalDealAmount()
        {
            var totalAmount = await _dealRepository.GetTotalDealAmountAsync();
            return Ok(BaseResponse<decimal>.Success(totalAmount));
        }

        [HttpGet("total-paid-amount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse<decimal>>> GetTotalPaidAmount()
        {
            var totalPaidAmount = await _dealRepository.GetTotalPaidAmountAsync();
            return Ok(BaseResponse<decimal>.Success(totalPaidAmount));
        }

        [HttpGet("total-remaining-amount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BaseResponse<decimal>>> GetTotalRemainingAmount()
        {
            var totalRemainingAmount = await _dealRepository.GetTotalRemainingAmountAsync();
            return Ok(BaseResponse<decimal>.Success(totalRemainingAmount));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResponse<DealDto>>> CreateDeal(CreateDealDto createDealDto)
        {
            try
            {
                var deal = _mapper.Map<Deal>(createDealDto);
                // Calculate tax amount
                deal.TaxAmount = deal.TotalAmount * (deal.TaxPercentage / 100);

                // Calculate remaining amount
                deal.RemainingAmount = deal.TotalAmount - deal.PaidAmount;

                var createdDeal = await _dealRepository.AddAsync(deal);

                // Create an activity record for the deal creation
                var activity = new DealActivity
                {
                    DealId = createdDeal.Id,
                    Type = ActivityType.Created,
                    Description = "Deal created",
                    ActivityDate = DateTime.UtcNow
                };

                await _dealActivityRepository.AddAsync(activity);

                var dealDto = _mapper.Map<DealDto>(createdDeal);
                return CreatedAtAction(nameof(GetDealById), new { id = createdDeal.Id },
                    BaseResponse<DealDto>.Success(dealDto, "Deal created successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating deal");
                return BadRequest(BaseResponse<DealDto>.Failure("Error creating deal", ex.Message));
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseResponse<object>>> UpdateDeal(Guid id, UpdateDealDto updateDealDto)
        {
            try
            {
                var deal = await _dealRepository.GetByIdAsync(id);
                if (deal == null)
                {
                    return NotFound(BaseResponse<object>.Failure($"Deal with ID {id} not found"));
                }

                // Store the old status to check if it changed
                var oldStatus = deal.Status;

                _mapper.Map(updateDealDto, deal);
                // Calculate tax amount
                deal.TaxAmount = deal.TotalAmount * (deal.TaxPercentage / 100);

                // Calculate remaining amount
                deal.RemainingAmount = deal.TotalAmount - deal.PaidAmount;

                await _dealRepository.UpdateAsync(deal);

                // Create an activity record for the deal update
                var activity = new DealActivity
                {
                    DealId = deal.Id,
                    Type = ActivityType.Updated,
                    Description = "Deal updated",
                    ActivityDate = DateTime.UtcNow
                };

                // If status changed, add a status change activity
                if (oldStatus != deal.Status)
                {
                    var statusActivity = new DealActivity
                    {
                        DealId = deal.Id,
                        Type = ActivityType.StatusChanged,
                        Description = $"Status changed from {oldStatus} to {deal.Status}",
                        ActivityDate = DateTime.UtcNow
                    };

                    await _dealActivityRepository.AddAsync(statusActivity);
                }

                await _dealActivityRepository.AddAsync(activity);

                return Ok(BaseResponse<object>.Success(new {}, "Deal updated successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating deal with ID {DealId}", id);
                return BadRequest(BaseResponse<object>.Failure("Error updating deal", ex.Message));
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseResponse<object>>> DeleteDeal(Guid id)
        {
            try
            {
                var deal = await _dealRepository.GetByIdAsync(id);
                if (deal == null)
                {
                    return NotFound(BaseResponse<object>.Failure($"Deal with ID {id} not found"));
                }

                await _dealRepository.SoftDeleteAsync(deal);

                // Create an activity record for the deal deletion
                var activity = new DealActivity
                {
                    DealId = deal.Id,
                    Type = ActivityType.Cancelled,
                    Description = "Deal deleted",
                    ActivityDate = DateTime.UtcNow
                };

                await _dealActivityRepository.AddAsync(activity);

                return Ok(BaseResponse<object>.Success(new {}, "Deal deleted successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting deal with ID {DealId}", id);
                return BadRequest(BaseResponse<object>.Failure("Error deleting deal", ex.Message));
            }
        }
    }
}
