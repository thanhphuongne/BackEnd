-- Sports Booking System Database Initialization Script
-- This script creates the database schema for the sports booking system

-- Create database (uncomment if needed)
-- CREATE DATABASE SportsBookingDB;
-- GO
-- USE SportsBookingDB;
-- GO

-- Create Sports table
CREATE TABLE Sports (
    Id int IDENTITY(1,1) PRIMARY KEY,
    SportName nvarchar(100) NOT NULL UNIQUE,
    Description nvarchar(500) NULL,
    Created datetimeoffset NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy nvarchar(max) NULL,
    LastModified datetimeoffset NOT NULL DEFAULT GETUTCDATE(),
    LastModifiedBy nvarchar(max) NULL
);

-- Create Fields table
CREATE TABLE Fields (
    Id int IDENTITY(1,1) PRIMARY KEY,
    SportId int NOT NULL,
    FieldName nvarchar(100) NOT NULL,
    Location nvarchar(200) NULL,
    Capacity int NULL,
    Description nvarchar(500) NULL,
    IsActive bit NOT NULL DEFAULT 1,
    Created datetimeoffset NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy nvarchar(max) NULL,
    LastModified datetimeoffset NOT NULL DEFAULT GETUTCDATE(),
    LastModifiedBy nvarchar(max) NULL,
    CONSTRAINT FK_Fields_Sports FOREIGN KEY (SportId) REFERENCES Sports(Id) ON DELETE CASCADE,
    CONSTRAINT UQ_Fields_SportId_FieldName UNIQUE (SportId, FieldName)
);

-- Create Users table
CREATE TABLE Users (
    Id int IDENTITY(1,1) PRIMARY KEY,
    UserName nvarchar(50) NOT NULL UNIQUE,
    Password nvarchar(255) NOT NULL,
    Phone nvarchar(20) NULL,
    Email nvarchar(100) NOT NULL UNIQUE,
    Role nvarchar(50) NOT NULL,
    Created datetimeoffset NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy nvarchar(max) NULL,
    LastModified datetimeoffset NOT NULL DEFAULT GETUTCDATE(),
    LastModifiedBy nvarchar(max) NULL
);

-- Create Pricings table
CREATE TABLE Pricings (
    Id int IDENTITY(1,1) PRIMARY KEY,
    SportId int NOT NULL,
    FieldId int NOT NULL,
    DayOfWeek int NOT NULL, -- 0=Sunday, 1=Monday, etc.
    StartTime time NOT NULL,
    EndTime time NOT NULL,
    PricePerHour decimal(18,2) NOT NULL,
    Created datetimeoffset NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy nvarchar(max) NULL,
    LastModified datetimeoffset NOT NULL DEFAULT GETUTCDATE(),
    LastModifiedBy nvarchar(max) NULL,
    CONSTRAINT FK_Pricings_Sports FOREIGN KEY (SportId) REFERENCES Sports(Id) ON DELETE CASCADE,
    CONSTRAINT FK_Pricings_Fields FOREIGN KEY (FieldId) REFERENCES Fields(Id),
    CONSTRAINT UQ_Pricings_Unique UNIQUE (SportId, FieldId, DayOfWeek, StartTime, EndTime)
);

-- Create Bookings table
CREATE TABLE Bookings (
    Id int IDENTITY(1,1) PRIMARY KEY,
    CustomerId int NOT NULL,
    FieldId int NOT NULL,
    SportId int NOT NULL,
    BookingDate date NOT NULL,
    StartTime time NOT NULL,
    EndTime time NOT NULL,
    TotalPrice decimal(18,2) NOT NULL,
    Status nvarchar(50) NOT NULL DEFAULT 'Pending',
    Created datetimeoffset NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy nvarchar(max) NULL,
    LastModified datetimeoffset NOT NULL DEFAULT GETUTCDATE(),
    LastModifiedBy nvarchar(max) NULL,
    CONSTRAINT FK_Bookings_Users FOREIGN KEY (CustomerId) REFERENCES Users(Id) ON DELETE CASCADE,
    CONSTRAINT FK_Bookings_Fields FOREIGN KEY (FieldId) REFERENCES Fields(Id),
    CONSTRAINT FK_Bookings_Sports FOREIGN KEY (SportId) REFERENCES Sports(Id)
);

-- Create indexes for better performance
CREATE INDEX IX_Fields_SportId ON Fields(SportId);
CREATE INDEX IX_Pricings_SportId ON Pricings(SportId);
CREATE INDEX IX_Pricings_FieldId ON Pricings(FieldId);
CREATE INDEX IX_Bookings_CustomerId ON Bookings(CustomerId);
CREATE INDEX IX_Bookings_FieldId ON Bookings(FieldId);
CREATE INDEX IX_Bookings_SportId ON Bookings(SportId);
CREATE INDEX IX_Bookings_BookingDate ON Bookings(BookingDate);
CREATE INDEX IX_Bookings_Field_Date_Time ON Bookings(FieldId, BookingDate, StartTime, EndTime);

-- Insert sample data
INSERT INTO Sports (SportName, Description) VALUES 
('Football', 'Association football, also known as soccer'),
('Basketball', 'Team sport played on a court with hoops'),
('Tennis', 'Racket sport played on a court'),
('Volleyball', 'Team sport played with a net'),
('Badminton', 'Racket sport played with shuttlecock');

INSERT INTO Fields (SportId, FieldName, Location, Capacity, Description, IsActive) VALUES 
(1, 'Main Football Field', 'North Campus', 22, 'Full-size football field with grass surface', 1),
(1, 'Training Field A', 'South Campus', 16, 'Smaller training field', 1),
(2, 'Basketball Court 1', 'Sports Center', 10, 'Indoor basketball court', 1),
(2, 'Basketball Court 2', 'Sports Center', 10, 'Indoor basketball court', 1),
(3, 'Tennis Court 1', 'Tennis Complex', 4, 'Hard court surface', 1),
(3, 'Tennis Court 2', 'Tennis Complex', 4, 'Clay court surface', 1),
(4, 'Volleyball Court', 'Sports Center', 12, 'Indoor volleyball court', 1),
(5, 'Badminton Court 1', 'Sports Center', 4, 'Indoor badminton court', 1),
(5, 'Badminton Court 2', 'Sports Center', 4, 'Indoor badminton court', 1);

INSERT INTO Users (UserName, Password, Phone, Email, Role) VALUES 
('admin', 'admin123', '+1234567890', 'admin@sportsbooking.com', 'Administrator'),
('john_doe', 'password123', '+1234567891', 'john.doe@email.com', 'Customer'),
('jane_smith', 'password123', '+1234567892', 'jane.smith@email.com', 'Customer'),
('manager1', 'manager123', '+1234567893', 'manager1@sportsbooking.com', 'Manager');

-- Sample pricing data (different rates for weekdays vs weekends)
INSERT INTO Pricings (SportId, FieldId, DayOfWeek, StartTime, EndTime, PricePerHour) VALUES 
-- Football - Weekdays
(1, 1, 1, '09:00', '17:00', 50.00), -- Monday
(1, 1, 2, '09:00', '17:00', 50.00), -- Tuesday
(1, 1, 3, '09:00', '17:00', 50.00), -- Wednesday
(1, 1, 4, '09:00', '17:00', 50.00), -- Thursday
(1, 1, 5, '09:00', '17:00', 50.00), -- Friday
-- Football - Weekends
(1, 1, 6, '09:00', '17:00', 75.00), -- Saturday
(1, 1, 0, '09:00', '17:00', 75.00), -- Sunday
-- Basketball - Weekdays
(2, 3, 1, '09:00', '22:00', 30.00), -- Monday
(2, 3, 2, '09:00', '22:00', 30.00), -- Tuesday
(2, 3, 3, '09:00', '22:00', 30.00), -- Wednesday
(2, 3, 4, '09:00', '22:00', 30.00), -- Thursday
(2, 3, 5, '09:00', '22:00', 30.00), -- Friday
-- Basketball - Weekends
(2, 3, 6, '09:00', '22:00', 45.00), -- Saturday
(2, 3, 0, '09:00', '22:00', 45.00); -- Sunday

PRINT 'Sports Booking Database initialized successfully!';
