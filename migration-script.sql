-- Migration Script: Consolidate AppUsers into Users table
-- This script moves data from AppUsers to Users table and drops AppUsers

-- Step 1: Add new columns to Users table (ASP.NET Identity table)
ALTER TABLE Users ADD 
    FullName nvarchar(100) NOT NULL DEFAULT '',
    DisplayName nvarchar(100) NULL,
    DateOfBirth datetime2 NULL,
    ProfilePhotoUrl nvarchar(255) NULL,
    AccountType nvarchar(max) NOT NULL DEFAULT 'Customer',
    VerificationCode nvarchar(10) NULL,
    LastLogin datetime2 NULL,
    Created datetimeoffset NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy nvarchar(max) NULL,
    LastModified datetimeoffset NOT NULL DEFAULT GETUTCDATE(),
    LastModifiedBy nvarchar(max) NULL;

-- Step 2: Migrate data from AppUsers to Users (if AppUsers table exists and has data)
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'AppUsers')
BEGIN
    -- Update Users table with data from AppUsers table
    UPDATE u SET 
        u.FullName = ISNULL(au.FullName, ''),
        u.DisplayName = au.DisplayName,
        u.PhoneNumber = au.Phone,
        u.DateOfBirth = au.DateOfBirth,
        u.ProfilePhotoUrl = au.ProfilePhotoUrl,
        u.AccountType = au.AccountType,
        u.VerificationCode = au.VerificationCode,
        u.LastLogin = au.LastLogin,
        u.Created = au.Created,
        u.CreatedBy = au.CreatedBy,
        u.LastModified = au.LastModified,
        u.LastModifiedBy = au.LastModifiedBy
    FROM Users u
    INNER JOIN AppUsers au ON u.Email = au.Email;
    
    -- Drop AppUsers table after migration
    DROP TABLE AppUsers;
END

-- Step 3: Create indexes for the new columns
CREATE UNIQUE NONCLUSTERED INDEX IX_Users_PhoneNumber 
ON Users (PhoneNumber) 
WHERE PhoneNumber IS NOT NULL;

-- Step 4: Remove the default constraint after migration
ALTER TABLE Users ALTER COLUMN FullName nvarchar(100) NOT NULL;

PRINT 'Migration completed: AppUsers data consolidated into Users table';
