using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Incentive.Application.Services;
using Incentive.Core.Common;
using Incentive.Core.Entities;
using Incentive.Core.Interfaces;
using Moq;
using Xunit;

namespace Incentive.UnitTests.Services
{
    public class IncentiveServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IRepository<Booking>> _mockBookingRepository;
        private readonly Mock<IRepository<IncentiveEarning>> _mockIncentiveEarningRepository;
        private readonly Mock<IRepository<IncentiveRule>> _mockIncentiveRuleRepository;
        private readonly IncentiveService _incentiveService;

        public IncentiveServiceTests()
        {
            _mockBookingRepository = new Mock<IRepository<Booking>>();
            _mockIncentiveEarningRepository = new Mock<IRepository<IncentiveEarning>>();
            _mockIncentiveRuleRepository = new Mock<IRepository<IncentiveRule>>();
            
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUnitOfWork.Setup(uow => uow.Repository<Booking>()).Returns(_mockBookingRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.Repository<IncentiveEarning>()).Returns(_mockIncentiveEarningRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.Repository<IncentiveRule>()).Returns(_mockIncentiveRuleRepository.Object);
            
            _incentiveService = new IncentiveService(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task CalculateIncentiveAsync_WithValidBooking_ShouldReturnIncentiveEarning()
        {
            // Arrange
            var bookingId = Guid.NewGuid();
            var salespersonId = Guid.NewGuid();
            var projectId = Guid.NewGuid();
            var ruleId = Guid.NewGuid();
            
            var booking = new Booking
            {
                Id = bookingId,
                SalespersonId = salespersonId,
                ProjectId = projectId,
                BookingValue = 1000m,
                Status = BookingStatus.Confirmed,
                BookingDate = DateTime.UtcNow
            };
            
            var rule = new IncentiveRule
            {
                Id = ruleId,
                ProjectId = projectId,
                Type = IncentiveType.Percentage,
                Value = 10m,
                IsActive = true,
                StartDate = DateTime.UtcNow.AddDays(-10),
                EndDate = DateTime.UtcNow.AddDays(10)
            };
            
            var incentiveEarning = new IncentiveEarning
            {
                Id = Guid.NewGuid(),
                BookingId = bookingId,
                SalespersonId = salespersonId,
                IncentiveRuleId = ruleId,
                Amount = 100m,
                Status = IncentiveEarningStatus.Pending
            };
            
            _mockBookingRepository.Setup(r => r.GetByIdAsync(bookingId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(booking);
            
            _mockIncentiveEarningRepository.Setup(r => r.AsQueryable())
                .Returns(new List<IncentiveEarning>().AsQueryable());
            
            _mockIncentiveRuleRepository.Setup(r => r.AsQueryable())
                .Returns(new List<IncentiveRule> { rule }.AsQueryable());
            
            _mockIncentiveEarningRepository.Setup(r => r.AddAsync(It.IsAny<IncentiveEarning>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(incentiveEarning);
            
            // Act
            var result = await _incentiveService.CalculateIncentiveAsync(bookingId);
            
            // Assert
            result.Should().NotBeNull();
            result.BookingId.Should().Be(bookingId);
            result.SalespersonId.Should().Be(salespersonId);
            result.IncentiveRuleId.Should().Be(ruleId);
            result.Amount.Should().Be(100m);
            result.Status.Should().Be(IncentiveEarningStatus.Pending);
            
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
        
        [Fact]
        public async Task CalculateIncentiveAsync_WithNonConfirmedBooking_ShouldThrowException()
        {
            // Arrange
            var bookingId = Guid.NewGuid();
            var booking = new Booking
            {
                Id = bookingId,
                Status = BookingStatus.Pending
            };
            
            _mockBookingRepository.Setup(r => r.GetByIdAsync(bookingId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(booking);
            
            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _incentiveService.CalculateIncentiveAsync(bookingId));
        }
    }
}
