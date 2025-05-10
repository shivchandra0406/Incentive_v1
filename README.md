# Incentive Management System

A multi-tenant incentive management system for tracking and calculating sales incentives.

## Architecture

This project follows a Hexagonal Architecture (also known as Ports and Adapters) pattern:

### Core Layer (Domain)
- Contains the business entities, interfaces (ports), and domain logic
- No dependencies on external frameworks or libraries
- Located in `src/Incentive.Core`

### Application Layer
- Contains the application services, DTOs, and use cases
- Depends only on the Core layer
- Located in `src/Incentive.Application`

### Infrastructure Layer
- Contains implementations of the interfaces defined in the Core layer
- Includes database access, external services, etc.
- Located in `src/Incentive.Infrastructure`

### API Layer
- Contains the API controllers and middleware
- Depends on all other layers
- Located in `src/Incentive.API`

## Features

- **Multi-tenancy**: Support for multiple tenants with isolated data
- **Authentication**: JWT-based authentication with refresh tokens
- **Authorization**: Role-based and claims-based authorization
- **Incentive Calculation**: Automatic calculation of incentives based on configurable rules
- **Reporting**: Generate reports on incentive earnings

## Technologies

- **.NET 8**: Modern C# features and performance improvements
- **Entity Framework Core**: ORM for database access
- **ASP.NET Core Identity**: User management and authentication
- **PostgreSQL**: Relational database
- **Swagger**: API documentation

## Getting Started

### Prerequisites

- .NET 8 SDK
- PostgreSQL

### Setup

1. Clone the repository
2. Update the connection string in `appsettings.json`
3. Run database migrations:
   ```
   dotnet ef database update
   ```
4. Run the application:
   ```
   dotnet run
   ```

## Project Structure

```
Incentive/
│
├── src/
│   ├── Incentive.Core/                         # Domain Layer (Hexagon)
│   │   ├── Entities/
│   │   │   ├── Incentive.cs
│   │   │   ├── IncentiveRule.cs
│   │   │   ├── Booking.cs
│   │   │   ├── Project.cs
│   │   │   ├── Salesperson.cs
│   │   │   ├── Tenant.cs
│   │   │   └── User.cs
│   │   ├── Interfaces/                         # Ports
│   │   │   ├── IIncentiveService.cs
│   │   │   ├── ITenantService.cs
│   │   │   ├── IAuthService.cs
│   │   │   └── IRepository.cs
│   │   ├── Enums/
│   │   ├── Exceptions/
│   │   └── Specifications/
│
│   ├── Incentive.Application/                  # Application Layer (Use Cases)
│   │   ├── DTOs/
│   │   │   ├── IncentiveDto.cs
│   │   │   ├── BookingDto.cs
│   │   │   └── AuthDto.cs
│   │   ├── Services/
│   │   │   ├── IncentiveService.cs
│   │   │   └── AuthService.cs
│   │   ├── Interfaces/
│   │   ├── Mappings/
│   │   ├── Validators/
│   │   └── UseCases/
│   │       ├── Incentives/
│   │       ├── Auth/
│   │       └── Tenants/
│
│   ├── Incentive.Infrastructure/               # Adapters (EF Core, Identity, etc.)
│   │   ├── Data/
│   │   │   ├── AppDbContext.cs
│   │   │   └── EntityConfigurations/
│   │   ├── Identity/
│   │   │   ├── AppUser.cs
│   │   │   ├── AppRole.cs
│   │   │   └── TokenService.cs
│   │   ├── MultiTenancy/
│   │   │   ├── TenantStore.cs
│   │   │   └── ITenantProvider.cs
│   │   ├── Repositories/
│   │   └── DependencyInjection.cs
│
│   ├── Incentive.API/                          # Presentation Layer
│   │   ├── Controllers/
│   │   │   ├── AuthController.cs
│   │   │   ├── IncentivesController.cs
│   │   │   └── TenantsController.cs
│   │   ├── Middleware/
│   │   │   ├── TenantResolutionMiddleware.cs
│   │   │   └── ExceptionMiddleware.cs
│   │   ├── Extensions/
│   │   ├── Program.cs
│   │   └── appsettings.json
│
├── tests/
│   ├── Incentive.UnitTests/
│   ├── Incentive.IntegrationTests/
│   └── Shared/
```

## License

This project is licensed under the MIT License - see the LICENSE file for details.
