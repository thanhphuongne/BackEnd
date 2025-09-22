# Sports Booking Management System - Backend API

A comprehensive .NET 8 Web API backend for sports facility booking management, supporting both customer and field owner applications. Built with Clean Architecture principles and modern .NET technologies.

## 🚀 Features

### Customer Features
- **User Authentication**: Registration, login, profile management, password recovery
- **Sports Discovery**: Browse sports, venues, and fields with availability
- **Booking System**: Real-time availability checking, pricing calculation, booking management
- **Match Organization**: Create and join organized matches and events
- **Team Management**: Form and manage sports teams
- **Reviews & Ratings**: Rate and review sports facilities
- **Dashboard**: Personal booking history and statistics

### Field Owner Features
- **Field Management**: Create, update, and manage sports facilities
- **Booking Oversight**: View and manage all bookings for owned fields
- **Analytics Dashboard**: Revenue tracking, popular times, customer insights
- **Payment Settings**: Configure payment methods and commission rates
- **Profile Management**: Business profile with contact information

### Technical Features
- **Clean Architecture**: Domain-Driven Design with clear separation of concerns
- **JWT Authentication**: Secure token-based authentication
- **Dynamic Pricing**: Flexible pricing rules by sport, time, and day
- **Real-time Availability**: Live booking availability checking
- **Comprehensive API**: RESTful endpoints with OpenAPI documentation
- **Database Seeding**: Sample data for development and testing

## 🏗️ Architecture

This project follows Clean Architecture principles:

```
src/
├── Domain/           # Core business entities and rules
├── Application/      # Use cases, DTOs, and business logic
├── Infrastructure/   # Data access, external services, identity
└── Web/             # API controllers, middleware, presentation
```

### Key Components

- **Domain Layer**: Entities, enums, value objects, and domain services
- **Application Layer**: CQRS with MediatR, validation, mapping
- **Infrastructure Layer**: EF Core, Identity, external APIs
- **Web Layer**: ASP.NET Core controllers, authentication, Swagger

## 🛠️ Technology Stack

- **Framework**: .NET 8, ASP.NET Core Web API
- **Database**: SQLite (development), SQL Server (production)
- **ORM**: Entity Framework Core with code-first migrations
- **Authentication**: ASP.NET Core Identity with JWT tokens
- **Validation**: FluentValidation
- **Mapping**: AutoMapper
- **Documentation**: NSwag (OpenAPI/Swagger)
- **Architecture**: MediatR for CQRS, Clean Architecture

## 📋 Prerequisites

- .NET 8.0 SDK
- SQLite (included) or SQL Server
- Visual Studio 2022 or VS Code

## 🚀 Quick Start

### 1. Clone and Setup
```bash
git clone <repository-url>
cd sports-booking-api
```

### 2. Database Setup
```bash
# Apply migrations
dotnet ef database update --project src/Infrastructure --startup-project src/Web

# Or run the application (auto-migrates in development)
dotnet run --project src/Web
```

### 3. Run the Application
```bash
cd src/Web
dotnet run
```

Navigate to `https://localhost:5001/swagger` for API documentation.

## 📚 API Documentation

### Authentication Endpoints
```
POST /api/auth/register          # User registration
POST /api/auth/login             # User login
POST /api/auth/refresh           # Refresh access token
GET  /api/auth/profile           # Get user profile
PUT  /api/auth/profile           # Update user profile
PUT  /api/auth/change-password   # Change password
POST /api/auth/forgot-password   # Request password reset
POST /api/auth/reset-password    # Reset password with code
```

### Customer Endpoints
```
# Sports & Fields
GET  /api/booking/sports         # Get all sports
GET  /api/booking/venues         # Get venues (by sport optional)
GET  /api/booking/fields         # Get fields (by sport optional)
GET  /api/booking/fields/{id}    # Get field details
GET  /api/booking/fields/{id}/availability  # Check availability

# Bookings
POST /api/booking/bookings       # Create booking
GET  /api/booking/bookings/my    # Get user's bookings
GET  /api/booking/bookings/{id}  # Get booking details
PUT  /api/booking/bookings/{id}/cancel  # Cancel booking

# Matches & Teams
GET  /api/matches                # Get all matches
POST /api/matches                # Create match
POST /api/matches/{id}/join      # Join match
GET  /api/teams                  # Get all teams
POST /api/teams                  # Create team

# Reviews & Dashboard
POST /api/reviews                # Create review
GET  /api/reviews/field/{id}     # Get reviews for field
GET  /api/dashboard              # User dashboard
GET  /api/dashboard/stats        # User statistics
```

### Field Owner Endpoints
```
# Field Management
GET  /api/owner/fields           # Get owner's fields
POST /api/owner/fields           # Create new field
GET  /api/owner/fields/{id}      # Get field details
PUT  /api/owner/fields/{id}      # Update field
DELETE /api/owner/fields/{id}    # Delete field

# Booking Management
GET  /api/owner/bookings         # Get all bookings for fields
GET  /api/owner/bookings/{id}    # Get booking details
PUT  /api/owner/bookings/{id}    # Update booking status
PUT  /api/owner/bookings/{id}/cancel  # Cancel booking

# Business Management
GET  /api/owner/analytics        # Get analytics
GET  /api/owner/payment-settings # Get payment settings
PUT  /api/owner/payment-settings # Update payment settings
POST /api/owner/upload-avatar    # Upload avatar
```

## 🗄️ Database Schema

### Core Entities
- **Users**: Customer and business owner accounts
- **Businesses**: Business entities owning venues
- **Venues**: Physical locations containing fields
- **Fields**: Individual sports facilities
- **Bookings**: Reservation records
- **Sports**: Sport types and categories
- **Matches**: Organized sporting events
- **Teams**: User-created sports teams
- **Reviews**: User feedback on facilities

### Key Relationships
```
User → Business (1:1, owners only)
Business → Venue (1:N)
Venue → Field (1:N)
Field → Booking (1:N)
User → Booking (1:N)
Sport → Field (1:N)
```

## 🔧 Configuration

### appsettings.json
```json
{
  "ConnectionStrings": {
    "BackEndDb": "Data Source=SportsBooking.db"
  },
  "JwtSettings": {
    "Secret": "your-jwt-secret-key",
    "ExpiryMinutes": 60,
    "RefreshTokenExpiryDays": 7
  }
}
```

### Environment Variables
```bash
ASPNETCORE_ENVIRONMENT=Development
ConnectionStrings__BackEndDb=Server=localhost;Database=SportsBooking;Trusted_Connection=True
JwtSettings__Secret=your-production-jwt-secret
```

## 🧪 Testing

### Using Postman
Import `Sports-Booking-API.postman_collection.json` for complete API testing.

### Sample Test Flow
1. Register a new user
2. Login to get JWT token
3. Browse available sports and fields
4. Check field availability
5. Create a booking
6. View booking history

## 📊 Sample Data

The application includes sample data for testing:
- 5 sports (Football, Basketball, Tennis, etc.)
- Multiple venues and fields
- Sample users (customers and owners)
- Pre-configured pricing rules

## 🚀 Deployment

### Docker
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY . .
EXPOSE 80
ENTRYPOINT ["dotnet", "src/Web/Web.dll"]
```

### Azure App Service
1. Publish to Azure
2. Configure connection strings
3. Set JWT secrets in Key Vault
4. Enable authentication if needed

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make changes with tests
4. Submit a pull request

### Code Standards
- Follow C# coding conventions
- Use meaningful variable names
- Add XML documentation comments
- Write unit tests for business logic

## 📝 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🆘 Support

For support and questions:
- Create an issue on GitHub
- Check the API documentation at `/swagger`
- Review the Postman collection examples

## 🔄 Version History

- **v1.0.0**: Initial release with core booking functionality
- **v1.1.0**: Added owner management features
- **v1.2.0**: Enhanced analytics and payment integration
- **v1.3.0**: Match organization and team management

---

Built with ❤️ using .NET 8 and Clean Architecture principles.