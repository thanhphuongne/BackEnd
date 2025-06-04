# Sports Booking System Database Setup Guide

## Overview
This guide provides instructions for setting up the SQL Server database for the sports booking system. The system includes entities for Sports, Fields, Users, Pricing, and Bookings.

## Database Schema

### Entities Created

1. **Sports** - Stores different types of sports
   - Id (Primary Key)
   - SportName (Unique)
   - Description
   - Audit fields (Created, CreatedBy, LastModified, LastModifiedBy)

2. **Fields** - Stores sports fields/venues
   - Id (Primary Key)
   - SportId (Foreign Key to Sports)
   - FieldName
   - Location
   - Capacity
   - Description
   - IsActive
   - Audit fields

3. **CustomUsers** - Stores user information (renamed to avoid conflict with Identity)
   - Id (Primary Key)
   - UserName (Unique)
   - Password
   - Phone
   - Email (Unique)
   - Role
   - Audit fields

4. **Pricings** - Stores pricing information for fields by day/time
   - Id (Primary Key)
   - SportId (Foreign Key to Sports)
   - FieldId (Foreign Key to Fields)
   - DayOfWeek (0=Sunday, 1=Monday, etc.)
   - StartTime
   - EndTime
   - PricePerHour
   - Audit fields

5. **Bookings** - Stores booking information
   - Id (Primary Key)
   - CustomerId (Foreign Key to CustomUsers)
   - FieldId (Foreign Key to Fields)
   - SportId (Foreign Key to Sports)
   - BookingDate
   - StartTime
   - EndTime
   - TotalPrice
   - Status (Default: "Pending")
   - Audit fields

## Setup Options

### Option 1: Using Entity Framework Migrations (Recommended for Development)

1. **Prerequisites:**
   - .NET 8.0 SDK installed
   - SQL Server instance running
   - Entity Framework tools installed

2. **Update Connection String:**
   Update the connection string in `src/Web/appsettings.Development.json`:
   ```json
   {
     "ConnectionStrings": {
       "BackEndDb": "Server=YOUR_SERVER;Database=SportsBookingDB;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True;MultipleActiveResultSets=true"
     }
   }
   ```

3. **Apply Migration:**
   ```bash
   cd /path/to/BackEnd
   dotnet ef database update --project src/Infrastructure --startup-project src/Web
   ```

### Option 2: Using SQL Script (Direct Database Creation)

Run the `database-init.sql` script directly in SQL Server Management Studio or any SQL Server client. This script:
- Creates all tables with proper constraints
- Sets up foreign key relationships
- Creates indexes for performance
- Inserts sample data

## Sample Data Included

The database initialization includes sample data:
- 5 sports (Football, Basketball, Tennis, Volleyball, Badminton)
- 9 fields across different sports
- 4 sample users (admin, customers, manager)
- Sample pricing data with different rates for weekdays/weekends

## Key Features

### Relationships
- Sports can have multiple Fields
- Fields can have multiple Pricings and Bookings
- Users can have multiple Bookings
- Proper cascade delete and restrict policies

### Indexes
- Unique constraints on SportName, UserName, Email
- Composite indexes for efficient querying
- Performance indexes on frequently queried columns

### Constraints
- Unique pricing per field/sport/day/time combination
- Unique field names per sport
- Default values for IsActive and Status fields

## Connection String Examples

### SQL Server (Windows Authentication)
```
Server=localhost;Database=SportsBookingDB;Trusted_Connection=True;MultipleActiveResultSets=true
```

### SQL Server (SQL Authentication)
```
Server=localhost,1433;Database=SportsBookingDB;User Id=sa;Password=YourPassword;TrustServerCertificate=True;MultipleActiveResultSets=true
```

### Azure SQL Database
```
Server=tcp:yourserver.database.windows.net,1433;Database=SportsBookingDB;User ID=yourusername;Password=yourpassword;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
```

## Troubleshooting

### Common Issues

1. **LocalDB not supported on Linux:**
   - Use regular SQL Server instead of LocalDB
   - Update connection string accordingly

2. **Migration fails:**
   - Ensure SQL Server is running
   - Check connection string
   - Verify user permissions

3. **Build errors:**
   - Ensure .NET 8.0 SDK is installed
   - Run `dotnet restore` to restore packages

## Next Steps

After database setup:
1. Test the connection by running the application
2. Verify sample data is loaded correctly
3. Create additional test data as needed
4. Set up proper user authentication
5. Implement business logic for booking validation

## Files Modified/Created

- `src/Domain/Entities/` - Entity classes
- `src/Infrastructure/Data/Configurations/` - EF configurations
- `src/Infrastructure/Data/ApplicationDbContext.cs` - Updated DbContext
- `src/Application/Common/Interfaces/IApplicationDbContext.cs` - Updated interface
- `database-init.sql` - Direct SQL script for database creation
- Migration files in `src/Infrastructure/Data/Migrations/`
