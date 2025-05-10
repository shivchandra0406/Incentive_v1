using System;
using FluentAssertions;
using Incentive.Domain.Entities;
using Incentive.Domain.Enums;
using Xunit;

namespace Incentive.UnitTests.Domain
{
    public class IncentiveRuleTests
    {
        [Fact]
        public void IncentiveRule_ShouldHaveCorrectProperties()
        {
            // Arrange
            var id = Guid.NewGuid();
            var projectId = Guid.NewGuid();
            var name = "Test Incentive Rule";
            var description = "Test Description";
            var type = IncentiveType.Percentage;
            var value = 10.5m;
            var minBookingValue = 1000m;
            var maxBookingValue = 5000m;
            var startDate = DateTime.UtcNow;
            var endDate = DateTime.UtcNow.AddDays(30);
            var isActive = true;

            // Act
            var incentiveRule = new IncentiveRule
            {
                Id = id,
                ProjectId = projectId,
                Name = name,
                Description = description,
                Type = type,
                Value = value,
                MinBookingValue = minBookingValue,
                MaxBookingValue = maxBookingValue,
                StartDate = startDate,
                EndDate = endDate,
                IsActive = isActive
            };

            // Assert
            incentiveRule.Id.Should().Be(id);
            incentiveRule.ProjectId.Should().Be(projectId);
            incentiveRule.Name.Should().Be(name);
            incentiveRule.Description.Should().Be(description);
            incentiveRule.Type.Should().Be(type);
            incentiveRule.Value.Should().Be(value);
            incentiveRule.MinBookingValue.Should().Be(minBookingValue);
            incentiveRule.MaxBookingValue.Should().Be(maxBookingValue);
            incentiveRule.StartDate.Should().Be(startDate);
            incentiveRule.EndDate.Should().Be(endDate);
            incentiveRule.IsActive.Should().Be(isActive);
        }

        [Fact]
        public void IncentiveRule_ShouldInheritFromSoftDeletableEntity()
        {
            // Arrange
            var incentiveRule = new IncentiveRule();

            // Act & Assert
            incentiveRule.Should().BeAssignableTo<Incentive.Domain.Common.SoftDeletableEntity>();
        }
    }
}
