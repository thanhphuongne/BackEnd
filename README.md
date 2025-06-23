# Sports Booking Management System - Backend API

A clean architecture .NET 8 Web API for managing sports facility bookings. This backend system provides comprehensive functionality for sports booking management including sports, fields, users, pricing, and bookings.

## Features

- **Sports Management**: Create and manage different types of sports
- **Field Management**: Manage sports fields with capacity, location, and availability
- **User Management**: Handle customer and staff user accounts
- **Dynamic Pricing**: Configure pricing by sport, field, day of week, and time slots
- **Booking System**: Complete booking management with status tracking
- **Clean Architecture**: Follows clean architecture principles with clear separation of concerns
- **Entity Framework Core**: SQL Server database with code-first migrations
- **OpenAPI/Swagger**: Comprehensive API documentation

## Database Entities

- **Sports**: Football, Basketball, Tennis, Volleyball, Badminton, etc.
- **Fields**: Sports venues with location, capacity, and availability status
- **Users**: Customer and staff accounts with role-based access
- **Pricing**: Flexible pricing rules by sport, field, day, and time
- **Bookings**: Reservation management with status tracking

## Build

Run `dotnet build` to build the solution.

## Run

To run the web API:

```bash
cd .\src\Web\
dotnet run
```

Navigate to https://localhost:5001/swagger to access the API documentation.

## Database Setup

1. **Update Connection String**: Configure your SQL Server connection in `src/Web/appsettings.Development.json`
2. **Apply Migrations**: Run `dotnet ef database update --project src/Infrastructure --startup-project src/Web`
3. **Sample Data**: The application will automatically seed sample sports, fields, users, and pricing data

For detailed database setup instructions, see [DATABASE_SETUP_GUIDE.md](DATABASE_SETUP_GUIDE.md).

## API Endpoints

The API provides endpoints for:
- `/api/sports` - Sports management
- `/api/fields` - Field management
- `/api/users` - User management
- `/api/pricing` - Pricing configuration
- `/api/bookings` - Booking operations

## Architecture

This project follows Clean Architecture principles:
- **Domain**: Core business entities and logic
- **Application**: Use cases and business rules
- **Infrastructure**: Data access and external services
- **Web**: API controllers and presentation layer

## Technology Stack

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- AutoMapper
- FluentValidation
- MediatR
- NSwag (OpenAPI/Swagger)