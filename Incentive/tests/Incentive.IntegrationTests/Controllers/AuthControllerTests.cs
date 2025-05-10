using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Incentive.Application.DTOs;
using Incentive.IntegrationTests.Fixtures;
using Xunit;

namespace Incentive.IntegrationTests.Controllers
{
    public class AuthControllerTests : IClassFixture<WebApplicationFactory>
    {
        private readonly HttpClient _client;

        public AuthControllerTests(WebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Register_WithValidData_ShouldReturnSuccess()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                UserName = "testuser",
                Email = "test@example.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!",
                FirstName = "Test",
                LastName = "User",
                TenantId = "default"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/register", registerDto);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var result = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Message.Should().Be("User registered successfully");
        }

        [Fact]
        public async Task Register_WithMismatchedPasswords_ShouldReturnBadRequest()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                UserName = "testuser",
                Email = "test@example.com",
                Password = "Password123!",
                ConfirmPassword = "DifferentPassword123!",
                FirstName = "Test",
                LastName = "User",
                TenantId = "default"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/register", registerDto);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var result = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Message.Should().Be("Passwords do not match");
        }
    }
}
