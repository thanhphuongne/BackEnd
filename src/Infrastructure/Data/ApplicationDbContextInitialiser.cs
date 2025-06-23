using BackEnd.Domain.Constants;
using BackEnd.Domain.Entities;
using BackEnd.Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BackEnd.Infrastructure.Data;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        await initialiser.InitialiseAsync();

        await initialiser.SeedAsync();
    }
}

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default roles
        var administratorRole = new IdentityRole(Roles.Administrator);

        if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await _roleManager.CreateAsync(administratorRole);
        }

        // Default users
        var administrator = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };

        if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await _userManager.CreateAsync(administrator, "Administrator1!");
            if (!string.IsNullOrWhiteSpace(administratorRole.Name))
            {
                await _userManager.AddToRolesAsync(administrator, new [] { administratorRole.Name });
            }
        }

        // Default data
        // Seed sports booking data if necessary
        if (!_context.Sports.Any())
        {
            var sports = new[]
            {
                new Sport { SportName = "Football", Description = "Association football, also known as soccer" },
                new Sport { SportName = "Basketball", Description = "Team sport played on a court with hoops" },
                new Sport { SportName = "Tennis", Description = "Racket sport played on a court" },
                new Sport { SportName = "Volleyball", Description = "Team sport played with a net" },
                new Sport { SportName = "Badminton", Description = "Racket sport played with shuttlecock" }
            };

            _context.Sports.AddRange(sports);
            await _context.SaveChangesAsync();

            // Add fields for each sport
            var fields = new[]
            {
                new Field { SportId = sports[0].Id, FieldName = "Main Football Field", Location = "North Campus", Capacity = 22, Description = "Full-size football field with grass surface", IsActive = true },
                new Field { SportId = sports[0].Id, FieldName = "Training Field A", Location = "South Campus", Capacity = 16, Description = "Smaller training field", IsActive = true },
                new Field { SportId = sports[1].Id, FieldName = "Basketball Court 1", Location = "Sports Center", Capacity = 10, Description = "Indoor basketball court", IsActive = true },
                new Field { SportId = sports[1].Id, FieldName = "Basketball Court 2", Location = "Sports Center", Capacity = 10, Description = "Indoor basketball court", IsActive = true },
                new Field { SportId = sports[2].Id, FieldName = "Tennis Court 1", Location = "Tennis Complex", Capacity = 4, Description = "Hard court surface", IsActive = true },
                new Field { SportId = sports[2].Id, FieldName = "Tennis Court 2", Location = "Tennis Complex", Capacity = 4, Description = "Clay court surface", IsActive = true },
                new Field { SportId = sports[3].Id, FieldName = "Volleyball Court", Location = "Sports Center", Capacity = 12, Description = "Indoor volleyball court", IsActive = true },
                new Field { SportId = sports[4].Id, FieldName = "Badminton Court 1", Location = "Sports Center", Capacity = 4, Description = "Indoor badminton court", IsActive = true },
                new Field { SportId = sports[4].Id, FieldName = "Badminton Court 2", Location = "Sports Center", Capacity = 4, Description = "Indoor badminton court", IsActive = true }
            };

            _context.Fields.AddRange(fields);
            await _context.SaveChangesAsync();

            // Add sample users
            var users = new[]
            {
                new User { UserName = "john_doe", Password = "password123", Phone = "+1234567891", Email = "john.doe@email.com", Role = "Customer" },
                new User { UserName = "jane_smith", Password = "password123", Phone = "+1234567892", Email = "jane.smith@email.com", Role = "Customer" },
                new User { UserName = "manager1", Password = "manager123", Phone = "+1234567893", Email = "manager1@sportsbooking.com", Role = "Manager" }
            };

            _context.CustomUsers.AddRange(users);
            await _context.SaveChangesAsync();

            // Add sample pricing data
            var pricings = new[]
            {
                // Football - Weekdays
                new Pricing { SportId = sports[0].Id, FieldId = fields[0].Id, DayOfWeek = DayOfWeek.Monday, StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(17, 0, 0), PricePerHour = 50.00m },
                new Pricing { SportId = sports[0].Id, FieldId = fields[0].Id, DayOfWeek = DayOfWeek.Tuesday, StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(17, 0, 0), PricePerHour = 50.00m },
                new Pricing { SportId = sports[0].Id, FieldId = fields[0].Id, DayOfWeek = DayOfWeek.Wednesday, StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(17, 0, 0), PricePerHour = 50.00m },
                new Pricing { SportId = sports[0].Id, FieldId = fields[0].Id, DayOfWeek = DayOfWeek.Thursday, StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(17, 0, 0), PricePerHour = 50.00m },
                new Pricing { SportId = sports[0].Id, FieldId = fields[0].Id, DayOfWeek = DayOfWeek.Friday, StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(17, 0, 0), PricePerHour = 50.00m },
                // Football - Weekends
                new Pricing { SportId = sports[0].Id, FieldId = fields[0].Id, DayOfWeek = DayOfWeek.Saturday, StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(17, 0, 0), PricePerHour = 75.00m },
                new Pricing { SportId = sports[0].Id, FieldId = fields[0].Id, DayOfWeek = DayOfWeek.Sunday, StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(17, 0, 0), PricePerHour = 75.00m },
                // Basketball - Weekdays
                new Pricing { SportId = sports[1].Id, FieldId = fields[2].Id, DayOfWeek = DayOfWeek.Monday, StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(22, 0, 0), PricePerHour = 30.00m },
                new Pricing { SportId = sports[1].Id, FieldId = fields[2].Id, DayOfWeek = DayOfWeek.Tuesday, StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(22, 0, 0), PricePerHour = 30.00m },
                new Pricing { SportId = sports[1].Id, FieldId = fields[2].Id, DayOfWeek = DayOfWeek.Wednesday, StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(22, 0, 0), PricePerHour = 30.00m },
                new Pricing { SportId = sports[1].Id, FieldId = fields[2].Id, DayOfWeek = DayOfWeek.Thursday, StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(22, 0, 0), PricePerHour = 30.00m },
                new Pricing { SportId = sports[1].Id, FieldId = fields[2].Id, DayOfWeek = DayOfWeek.Friday, StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(22, 0, 0), PricePerHour = 30.00m },
                // Basketball - Weekends
                new Pricing { SportId = sports[1].Id, FieldId = fields[2].Id, DayOfWeek = DayOfWeek.Saturday, StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(22, 0, 0), PricePerHour = 45.00m },
                new Pricing { SportId = sports[1].Id, FieldId = fields[2].Id, DayOfWeek = DayOfWeek.Sunday, StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(22, 0, 0), PricePerHour = 45.00m }
            };

            _context.Pricings.AddRange(pricings);
            await _context.SaveChangesAsync();
        }
    }
}
