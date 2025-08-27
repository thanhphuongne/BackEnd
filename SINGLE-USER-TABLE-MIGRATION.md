# Single User Table Migration Guide

## 🎯 **Problem Solved**

Previously, your system had **2 user tables**:
- `Users` table (ASP.NET Identity) - Authentication only
- `AppUsers` table (Business logic) - User data

This caused complexity and data duplication. Now you have **1 unified `Users` table**.

## 🔄 **Changes Made**

### 1. **Extended ApplicationUser**
```csharp
public class ApplicationUser : IdentityUser
{
    // Business properties added
    public string FullName { get; set; } = null!;
    public string? DisplayName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public AccountType AccountType { get; set; } = AccountType.Customer;
    // ... other properties
}
```

### 2. **Updated AuthService**
- Registration now creates only 1 user record in `Users` table
- Login updates the same `Users` table
- No more dual-table management

### 3. **Database Schema**
The `Users` table now contains:
```sql
-- ASP.NET Identity columns (existing)
Id, UserName, Email, PhoneNumber, PasswordHash, etc.

-- Business columns (added)
FullName, DisplayName, DateOfBirth, AccountType, 
ProfilePhotoUrl, VerificationCode, LastLogin,
Created, CreatedBy, LastModified, LastModifiedBy
```

## 🚀 **Migration Steps**

### Option 1: Fresh Database (Recommended for Development)
```bash
# Delete existing database
# Run the application - it will create the new unified schema
cd src/Web
dotnet run
```

### Option 2: Migrate Existing Data
```bash
# Run the migration script
sqlcmd -S your-server -d SportsBookingDB -i migration-script.sql

# Then run the application
cd src/Web
dotnet run
```

## ✅ **Benefits**

1. **Simplified Architecture**: One user table instead of two
2. **Better Performance**: No joins between user tables
3. **Easier Maintenance**: Single source of truth for user data
4. **Cleaner Code**: No dual-entity management in services
5. **Frontend Ready**: Same API endpoints, same responses

## 🧪 **Testing**

Your Postman collection will work exactly the same:

**Request:**
```json
POST /api/auth/register
{
  "email": "test@example.com",
  "password": "Password123!",
  "confirmPassword": "Password123!",
  "fullName": "Test User",
  "accountType": 0
}
```

**Response:** (Same as before)
```json
{
  "token": "jwt-token...",
  "user": {
    "email": "test@example.com",
    "fullName": "Test User",
    "accountType": "Customer"
  }
}
```

## 📊 **Database Changes**

### Before:
```
Users (Identity)          AppUsers (Business)
├── Id                   ├── Id  
├── Email                ├── Email
├── PasswordHash         ├── FullName
└── ...                  ├── AccountType
                         └── ...
```

### After:
```
Users (Unified)
├── Id (Identity)
├── Email (Identity)  
├── PasswordHash (Identity)
├── FullName (Business)
├── AccountType (Business)
├── DisplayName (Business)
└── ... (all properties in one table)
```

## 🎉 **Result**

- ✅ **Single `Users` table** for all user data
- ✅ **Same API behavior** for frontend
- ✅ **Simplified codebase** with better maintainability
- ✅ **Better performance** with no table joins
- ✅ **All features work** exactly as before

Your register feature now saves everything to the single `Users` table, making it much cleaner and more efficient!
