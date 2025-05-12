using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Incentive.API.Attributes;
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
        public async Task<ActionResult<IEnumerable<DealDto>>> GetAllDeals()
        {
            var deals = await _dealRepository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<DealDto>>(deals));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<DealDto>> GetDealById(Guid id)
        {
            var deal = await _dealRepository.GetByIdAsync(id);
            if (deal == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<DealDto>(deal));
        }

        [HttpGet("status/{status}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<DealDto>>> GetDealsByStatus(DealStatus status)
        {
            var deals = await _dealRepository.GetDealsByStatusAsync(status);
            return Ok(_mapper.Map<IEnumerable<DealDto>>(deals));
        }

        [HttpGet("user/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<DealDto>>> GetDealsByUserId(Guid userId)
        {
            var deals = await _dealRepository.GetDealsByUserIdAsync(userId);
            return Ok(_mapper.Map<IEnumerable<DealDto>>(deals));
        }

        [HttpGet("date-range")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<DealDto>>> GetDealsByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate > endDate)
            {
                return BadRequest("Start date cannot be later than end date");
            }

            var deals = await _dealRepository.GetDealsByDateRangeAsync(startDate, endDate);
            return Ok(_mapper.Map<IEnumerable<DealDto>>(deals));
        }

        [HttpGet("incentive-rule/{incentiveRuleId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<DealDto>>> GetDealsByIncentiveRuleId(Guid incentiveRuleId)
        {
            var deals = await _dealRepository.GetDealsByIncentiveRuleIdAsync(incentiveRuleId);
            return Ok(_mapper.Map<IEnumerable<DealDto>>(deals));
        }

        [HttpGet("customer/{customerName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<DealDto>>> GetDealsByCustomerName(string customerName)
        {
            var deals = await _dealRepository.GetDealsByCustomerNameAsync(customerName);
            return Ok(_mapper.Map<IEnumerable<DealDto>>(deals));
        }

        [HttpGet("source/{source}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<DealDto>>> GetDealsBySource(string source)
        {
            var deals = await _dealRepository.GetDealsBySourceAsync(source);
            return Ok(_mapper.Map<IEnumerable<DealDto>>(deals));
        }

        [HttpGet("pending-payments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<DealDto>>> GetDealsWithPendingPayments()
        {
            var deals = await _dealRepository.GetDealsWithPendingPaymentsAsync();
            return Ok(_mapper.Map<IEnumerable<DealDto>>(deals));
        }

        [HttpGet("total-amount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<decimal>> GetTotalDealAmount()
        {
            var totalAmount = await _dealRepository.GetTotalDealAmountAsync();
            return Ok(totalAmount);
        }

        [HttpGet("total-paid-amount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<decimal>> GetTotalPaidAmount()
        {
            var totalPaidAmount = await _dealRepository.GetTotalPaidAmountAsync();
            return Ok(totalPaidAmount);
        }

        [HttpGet("total-remaining-amount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<decimal>> GetTotalRemainingAmount()
        {
            var totalRemainingAmount = await _dealRepository.GetTotalRemainingAmountAsync();
            return Ok(totalRemainingAmount);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DealDto>> CreateDeal(CreateDealDto createDealDto)
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

            return CreatedAtAction(nameof(GetDealById), new { id = createdDeal.Id }, _mapper.Map<DealDto>(createdDeal));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateDeal(Guid id, UpdateDealDto updateDealDto)
        {
            var deal = await _dealRepository.GetByIdAsync(id);
            if (deal == null)
            {
                return NotFound();
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

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteDeal(Guid id)
        {
            var deal = await _dealRepository.GetByIdAsync(id);
            if (deal == null)
            {
                return NotFound();
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

            return NoContent();
        }
    }
}
