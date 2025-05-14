using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Incentive.API.Attributes;
using Incentive.Application.DTOs;
using Incentive.Core.Entities;
using Incentive.Core.Enums;
using Incentive.Core.Interfaces;
using Incentive.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Incentive.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IDealRepository _dealRepository;
        private readonly IDealActivityRepository _dealActivityRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentsController> _logger;
        private readonly ICurrentUserService _currentUserService;

        public PaymentsController(
            IPaymentRepository paymentRepository,
            IDealRepository dealRepository,
            IDealActivityRepository dealActivityRepository,
            IMapper mapper,
            ILogger<PaymentsController> logger,
            ICurrentUserService currentUserService)
        {
            _paymentRepository = paymentRepository;
            _dealRepository = dealRepository;
            _dealActivityRepository = dealActivityRepository;
            _mapper = mapper;
            _logger = logger;
            _currentUserService = currentUserService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [RequiresTenantId(description: "The tenant ID to access data from")]
        public async Task<ActionResult<IEnumerable<PaymentDto>>> GetAllPayments()
        {
            var payments = await _paymentRepository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<PaymentDto>>(payments));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequiresTenantId(description: "The tenant ID to access data from")]
        public async Task<ActionResult<PaymentDto>> GetPaymentById(Guid id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PaymentDto>(payment));
        }

        [HttpGet("deal/{dealId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [RequiresTenantId(description: "The tenant ID to access data from")]
        public async Task<ActionResult<IEnumerable<PaymentDto>>> GetPaymentsByDealId(Guid dealId)
        {
            var payments = await _paymentRepository.GetPaymentsByDealIdAsync(dealId);
            return Ok(_mapper.Map<IEnumerable<PaymentDto>>(payments));
        }

        [HttpGet("date-range")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [RequiresTenantId(description: "The tenant ID to access data from")]
        public async Task<ActionResult<IEnumerable<PaymentDto>>> GetPaymentsByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate > endDate)
            {
                return BadRequest("Start date cannot be later than end date");
            }

            var payments = await _paymentRepository.GetPaymentsByDateRangeAsync(startDate, endDate);
            return Ok(_mapper.Map<IEnumerable<PaymentDto>>(payments));
        }

        [HttpGet("method/{paymentMethod}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [RequiresTenantId(description: "The tenant ID to access data from")]
        public async Task<ActionResult<IEnumerable<PaymentDto>>> GetPaymentsByMethod(string paymentMethod)
        {
            var payments = await _paymentRepository.GetPaymentsByMethodAsync(paymentMethod);
            return Ok(_mapper.Map<IEnumerable<PaymentDto>>(payments));
        }

        [HttpGet("user/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [RequiresTenantId(description: "The tenant ID to access data from")]
        public async Task<ActionResult<IEnumerable<PaymentDto>>> GetPaymentsByReceivedByUserId(string userId)
        {
            var payments = await _paymentRepository.GetPaymentsByReceivedByUserIdAsync(userId);
            return Ok(_mapper.Map<IEnumerable<PaymentDto>>(payments));
        }

        [HttpGet("unverified")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [RequiresTenantId(description: "The tenant ID to access data from")]
        public async Task<ActionResult<IEnumerable<PaymentDto>>> GetUnverifiedPayments()
        {
            var payments = await _paymentRepository.GetUnverifiedPaymentsAsync();
            return Ok(_mapper.Map<IEnumerable<PaymentDto>>(payments));
        }

        [HttpGet("total-amount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [RequiresTenantId(description: "The tenant ID to access data from")]
        public async Task<ActionResult<decimal>> GetTotalPaymentsAmount()
        {
            var totalAmount = await _paymentRepository.GetTotalPaymentsAmountAsync();
            return Ok(totalAmount);
        }

        [HttpGet("total-amount/deal/{dealId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [RequiresTenantId(description: "The tenant ID to access data from")]
        public async Task<ActionResult<decimal>> GetTotalPaymentsAmountByDealId(Guid dealId)
        {
            var totalAmount = await _paymentRepository.GetTotalPaymentsAmountByDealIdAsync(dealId);
            return Ok(totalAmount);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequiresTenantId(description: "The tenant ID to access data from")]
        public async Task<ActionResult<PaymentDto>> CreatePayment(CreatePaymentDto createPaymentDto)
        {
            // Check if the deal exists
            var deal = await _dealRepository.GetByIdAsync(createPaymentDto.DealId);
            if (deal == null)
            {
                return NotFound($"Deal with ID {createPaymentDto.DealId} not found");
            }

            var payment = _mapper.Map<Payment>(createPaymentDto);
            var createdPayment = await _paymentRepository.AddAsync(payment);

            // Update the deal's paid amount and remaining amount
            deal.PaidAmount += payment.Amount;
            deal.RemainingAmount = deal.TotalAmount - deal.PaidAmount;

            var userId = _currentUserService.GetUserId();
            _ = Guid.TryParse(userId, out Guid GuidUserId);
            // If the deal is fully paid, update its status
            if (deal.RemainingAmount <= 0)
            {
                deal.Status = DealStatus.FullyPaid;
                deal.ClosedDate = DateTime.UtcNow;
                deal.ClosedByUserId = userId;
            }

            await _dealRepository.UpdateAsync(deal);

            // Create an activity record for the payment
            var activity = new DealActivity
            {
                DealId = deal.Id,
                Type = ActivityType.PaymentReceived,
                Description = $"Payment of {payment.Amount} received via {payment.PaymentMethod}",
                Notes = payment.Notes,
                ActivityDate = DateTime.UtcNow
            };

            await _dealActivityRepository.AddAsync(activity);

            return CreatedAtAction(nameof(GetPaymentById), new { id = createdPayment.Id }, _mapper.Map<PaymentDto>(createdPayment));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequiresTenantId(description: "The tenant ID to access data from")]
        public async Task<IActionResult> UpdatePayment(Guid id, UpdatePaymentDto updatePaymentDto)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            // Store the old amount to calculate the difference
            var oldAmount = payment.Amount;

            _mapper.Map(updatePaymentDto, payment);

            await _paymentRepository.UpdateAsync(payment);

            // If the amount changed, update the deal's paid amount and remaining amount
            if (oldAmount != payment.Amount)
            {
                var deal = await _dealRepository.GetByIdAsync(payment.DealId);
                if (deal != null)
                {
                    // Adjust the paid amount by the difference
                    deal.PaidAmount = deal.PaidAmount - oldAmount + payment.Amount;
                    deal.RemainingAmount = deal.TotalAmount - deal.PaidAmount;


                    // If the deal is fully paid, update its status
                    if (deal.RemainingAmount <= 0 && deal.Status != DealStatus.FullyPaid)
                    {
                        deal.Status = DealStatus.FullyPaid;
                        deal.ClosedDate = DateTime.UtcNow;
                        deal.ClosedByUserId = deal.ClosedByUserId;
                    }
                    // If the deal was fully paid but now has a remaining amount, update its status
                    else if (deal.RemainingAmount > 0 && deal.Status == DealStatus.FullyPaid)
                    {
                        deal.Status = DealStatus.New;
                        deal.ClosedDate = null;
                    }

                    await _dealRepository.UpdateAsync(deal);

                    // Create an activity record for the payment update
                    var activity = new DealActivity
                    {
                        DealId = deal.Id,
                        Type = ActivityType.Updated,
                        Description = $"Payment updated from {oldAmount} to {payment.Amount}",
                        ActivityDate = DateTime.UtcNow
                    };

                    await _dealActivityRepository.AddAsync(activity);
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequiresTenantId(description: "The tenant ID to access data from")]
        public async Task<IActionResult> DeletePayment(Guid id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            // Store the payment amount and deal ID before deleting
            var amount = payment.Amount;
            var dealId = payment.DealId;

            await _paymentRepository.SoftDeleteAsync(payment);

            // Update the deal's paid amount and remaining amount
            var deal = await _dealRepository.GetByIdAsync(dealId);
            if (deal != null)
            {
                deal.PaidAmount -= amount;
                deal.RemainingAmount = deal.TotalAmount - deal.PaidAmount;

                // If the deal was fully paid but now has a remaining amount, update its status
                if (deal.RemainingAmount > 0 && deal.Status == DealStatus.FullyPaid)
                {
                    deal.Status = DealStatus.New;
                    deal.ClosedDate = null;
                }

                await _dealRepository.UpdateAsync(deal);

                // Create an activity record for the payment deletion
                var activity = new DealActivity
                {
                    DealId = deal.Id,
                    Type = ActivityType.Cancelled,
                    Description = $"Payment of {amount} deleted",
                    ActivityDate = DateTime.UtcNow
                };

                await _dealActivityRepository.AddAsync(activity);
            }

            return NoContent();
        }

        [HttpPut("{id}/verify")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [RequiresTenantId(description: "The tenant ID to access data from")]
        public async Task<IActionResult> VerifyPayment(Guid id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            payment.IsVerified = true;
            await _paymentRepository.UpdateAsync(payment);

            // Create an activity record for the payment verification
            var activity = new DealActivity
            {
                DealId = payment.DealId,
                Type = ActivityType.Updated,
                Description = $"Payment of {payment.Amount} verified",
                ActivityDate = DateTime.UtcNow
            };

            await _dealActivityRepository.AddAsync(activity);

            return NoContent();
        }
    }
}
