using BackEnd.Domain.Entities;
using BackEnd.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.Infrastructure.Data;

public class DataSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DataSeeder> _logger;

    public DataSeeder(ApplicationDbContext context, ILogger<DataSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        _logger.LogInformation("🌱 Starting data seeding...");

        await SeedSportsAsync();
        await SeedBusinessOwnerAsync();
        await SeedBusinessesAsync();
        await SeedVenuesAsync();
        await SeedFieldsAsync();

        _logger.LogInformation("✅ Data seeding completed!");
    }

    private async Task SeedSportsAsync()
    {
        var existingSports = await _context.Sports.Select(s => s.SportName).ToListAsync();
        var sportsToAdd = new List<Sport>();

        var allSports = new List<(string Name, string Description)>
        {
            ("Soccer", "Association football (soccer)"),
            ("Basketball", "Indoor and outdoor basketball courts"),
            ("Tennis", "Tennis courts for singles and doubles"),
            ("Badminton", "Indoor badminton courts"),
            ("Pickleball", "Indoor and outdoor pickleball courts")
        };

        foreach (var (name, desc) in allSports)
        {
            if (!existingSports.Contains(name))
            {
                sportsToAdd.Add(new Sport { SportName = name, Description = desc });
            }
        }

        if (sportsToAdd.Any())
        {
            _context.Sports.AddRange(sportsToAdd);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ Seeded {Count} new sports", sportsToAdd.Count);
        }
        else
        {
            _logger.LogInformation("🏃 All sports already exist, skipping...");
        }
    }

    private async Task SeedBusinessOwnerAsync()
    {
        if (await _context.Users.AnyAsync(u => u.AccountType == AccountType.FieldOwner))
        {
            _logger.LogInformation("👤 Business owner already exists, skipping...");
            return;
        }

        var businessOwner = new User
        {
            Email = "owner@sportsbooking.com",
            FullName = "Sports Facility Owner",
            DisplayName = "Owner",
            Phone = "+1234567890",
            AccountType = AccountType.FieldOwner,
            PasswordHash = "dummy-hash" // This would be set by Identity in real registration
        };

        _context.Users.Add(businessOwner);
        await _context.SaveChangesAsync();

        _logger.LogInformation("✅ Seeded business owner user");
    }

    private async Task SeedBusinessesAsync()
    {
        if (await _context.Businesses.AnyAsync())
        {
            _logger.LogInformation("🏢 Businesses already exist, skipping...");
            return;
        }

        var businesses = new List<Business>
        {
            new()
            {
                OwnerId = 1, // We'll need to create a business owner user first
                BusinessName = "City Sports Complex",
                ContactEmail = "info@citysports.com",
                ContactPhone = "+1234567890",
                Address = "123 Sports Avenue, City Center"
            },
            new()
            {
                OwnerId = 1,
                BusinessName = "Elite Tennis Club",
                ContactEmail = "contact@elitetennis.com",
                ContactPhone = "+1234567891",
                Address = "456 Tennis Road, Uptown"
            },
            new()
            {
                OwnerId = 1,
                BusinessName = "Community Recreation Center",
                ContactEmail = "info@communityrec.com",
                ContactPhone = "+1234567892",
                Address = "789 Community Drive, Downtown"
            }
        };

        _context.Businesses.AddRange(businesses);
        await _context.SaveChangesAsync();

        _logger.LogInformation("✅ Seeded {Count} businesses", businesses.Count);
    }

    private async Task SeedVenuesAsync()
    {
        if (await _context.Venues.AnyAsync())
        {
            _logger.LogInformation("🏟️ Venues already exist, skipping...");
            return;
        }

        var businesses = await _context.Businesses.ToListAsync();
        if (!businesses.Any())
        {
            _logger.LogWarning("❌ No businesses found for venue seeding");
            return;
        }

        var venues = new List<Venue>
        {
            // City Sports Complex venues
            new()
            {
                BusinessId = businesses[0].Id,
                VenueName = "Quy Nhon Stadium",
                Address = "123 Sports Avenue, Quy Nhon City Center",
                Description = "Premier stadium with multiple soccer fields and facilities",
                ContactPhone = "+84234567890",
                ContactEmail = "stadium@quynhon.com",
                OperatingHours = "06:00-22:00",
                Facilities = "Parking, Restrooms, Cafeteria, Locker Rooms, Medical Room",
                IsActive = true
            },
            new()
            {
                BusinessId = businesses[0].Id,
                VenueName = "City Sports Indoor Complex",
                Address = "125 Sports Avenue, Quy Nhon City Center",
                Description = "Indoor sports complex with basketball and badminton courts",
                ContactPhone = "+84234567891",
                ContactEmail = "indoor@citysports.com",
                OperatingHours = "06:00-23:00",
                Facilities = "Air Conditioning, Parking, Restrooms, Equipment Rental",
                IsActive = true
            },

            // Elite Tennis Club venues
            new()
            {
                BusinessId = businesses[1].Id,
                VenueName = "Elite Tennis Center",
                Address = "456 Tennis Road, Quy Nhon Uptown",
                Description = "Professional tennis facility with multiple court types",
                ContactPhone = "+84234567892",
                ContactEmail = "center@elitetennis.com",
                OperatingHours = "06:00-21:00",
                Facilities = "Pro Shop, Coaching, Parking, Clubhouse, Showers",
                IsActive = true
            },

            // Community Recreation Center venues
            new()
            {
                BusinessId = businesses[2].Id,
                VenueName = "Community Sports Hub",
                Address = "789 Community Drive, Quy Nhon Downtown",
                Description = "Affordable community sports facility",
                ContactPhone = "+84234567893",
                ContactEmail = "hub@communityrec.com",
                OperatingHours = "08:00-20:00",
                Facilities = "Basic Facilities, Parking, Restrooms",
                IsActive = true
            }
        };

        _context.Venues.AddRange(venues);
        await _context.SaveChangesAsync();

        _logger.LogInformation("✅ Seeded {Count} venues", venues.Count);
    }

    private async Task SeedFieldsAsync()
    {
        var venues = await _context.Venues.Include(v => v.Business).ToListAsync();
        if (!venues.Any())
        {
            _logger.LogWarning("❌ No venues found for field seeding");
            return;
        }

        var existingFieldNames = await _context.Fields.Select(f => f.FieldName).ToListAsync();
        var fieldsToAdd = new List<Field>();

        // Define all fields to potentially add
        var allFields = new List<Field>();

        // Use the first venue for all fields to avoid index out of range
        var defaultVenue = venues.FirstOrDefault();
        if (defaultVenue == null)
        {
            _logger.LogWarning("❌ No venues available for field seeding");
            return;
        }

        // Existing fields (add if not exist)
        allFields.AddRange(new List<Field>
        {
            new()
            {
                BusinessId = defaultVenue.BusinessId,
                VenueId = defaultVenue.Id,
                FieldName = "Main Soccer Field",
                FieldNumber = "Field 1",
                SportType = SportType.Soccer,
                Description = "Full-size soccer field with natural grass and professional lighting",
                BasePrice = 80.00m,
                OperatingHours = "06:00-22:00",
                IsActive = true
            },
            new()
            {
                BusinessId = defaultVenue.BusinessId,
                VenueId = defaultVenue.Id,
                FieldName = "Training Soccer Field",
                FieldNumber = "Field 2",
                SportType = SportType.Soccer,
                Description = "Training field with artificial turf",
                BasePrice = 60.00m,
                OperatingHours = "06:00-22:00",
                IsActive = true
            },
            new()
            {
                BusinessId = defaultVenue.BusinessId,
                VenueId = defaultVenue.Id,
                FieldName = "Youth Soccer Field",
                FieldNumber = "Field 3",
                SportType = SportType.Soccer,
                Description = "Smaller field perfect for youth training",
                BasePrice = 40.00m,
                OperatingHours = "06:00-22:00",
                IsActive = true
            },
            new()
            {
                BusinessId = defaultVenue.BusinessId,
                VenueId = defaultVenue.Id,
                FieldName = "Basketball Court",
                FieldNumber = "Court A",
                SportType = SportType.Basketball,
                Description = "Professional indoor basketball court with wooden flooring",
                BasePrice = 50.00m,
                OperatingHours = "06:00-23:00",
                IsActive = true
            },
            new()
            {
                BusinessId = defaultVenue.BusinessId,
                VenueId = defaultVenue.Id,
                FieldName = "Basketball Court",
                FieldNumber = "Court B",
                SportType = SportType.Basketball,
                Description = "Secondary basketball court for training",
                BasePrice = 40.00m,
                OperatingHours = "06:00-23:00",
                IsActive = true
            },
            new()
            {
                BusinessId = defaultVenue.BusinessId,
                VenueId = defaultVenue.Id,
                FieldName = "Badminton Court",
                FieldNumber = "Court 1",
                SportType = SportType.Badminton,
                Description = "Professional badminton court with wooden flooring",
                BasePrice = 30.00m,
                OperatingHours = "07:00-22:00",
                IsActive = true
            },
            new()
            {
                BusinessId = defaultVenue.BusinessId,
                VenueId = defaultVenue.Id,
                FieldName = "Badminton Court",
                FieldNumber = "Court 2",
                SportType = SportType.Badminton,
                Description = "Secondary badminton court",
                BasePrice = 30.00m,
                OperatingHours = "07:00-22:00",
                IsActive = true
            },
            new()
            {
                BusinessId = defaultVenue.BusinessId,
                VenueId = defaultVenue.Id,
                FieldName = "Tennis Court",
                FieldNumber = "Court 1",
                SportType = SportType.Tennis,
                Description = "Professional clay tennis court",
                BasePrice = 60.00m,
                OperatingHours = "06:00-21:00",
                IsActive = true
            },
            new()
            {
                BusinessId = defaultVenue.BusinessId,
                VenueId = defaultVenue.Id,
                FieldName = "Tennis Court",
                FieldNumber = "Court 2",
                SportType = SportType.Tennis,
                Description = "Hard court tennis court with lights",
                BasePrice = 50.00m,
                OperatingHours = "06:00-22:00",
                IsActive = true
            },
            new()
            {
                BusinessId = defaultVenue.BusinessId,
                VenueId = defaultVenue.Id,
                FieldName = "Multi-Purpose Court",
                FieldNumber = "Court 1",
                SportType = SportType.Basketball,
                Description = "Multi-purpose court for basketball and community events",
                BasePrice = 25.00m,
                OperatingHours = "08:00-20:00",
                IsActive = true
            },
            new()
            {
                BusinessId = defaultVenue.BusinessId,
                VenueId = defaultVenue.Id,
                FieldName = "Badminton Court",
                FieldNumber = "Court 1",
                SportType = SportType.Badminton,
                Description = "Community badminton court",
                BasePrice = 20.00m,
                OperatingHours = "08:00-21:00",
                IsActive = true
            }
        });

        // Add 5 Badminton fields, each with 3 small fields
        for (int i = 1; i <= 5; i++)
        {
            allFields.Add(new Field
            {
                BusinessId = defaultVenue.BusinessId,
                VenueId = defaultVenue.Id,
                FieldName = $"Badminton Field {i}",
                FieldNumber = $"Field {i}",
                SportType = SportType.Badminton,
                Description = $"Main badminton field {i}",
                BasePrice = 30.00m,
                OperatingHours = "07:00-22:00",
                IsActive = true
            });

            // Add 3 small fields for each main field
            foreach (char sub in new[] { 'A', 'B', 'C' })
            {
                allFields.Add(new Field
                {
                    BusinessId = defaultVenue.BusinessId,
                    VenueId = defaultVenue.Id,
                    FieldName = $"Badminton Field {i} - Small Field {sub}",
                    FieldNumber = $"Field {i}{sub}",
                    SportType = SportType.Badminton,
                    Description = $"Small badminton field {sub} in Field {i}",
                    BasePrice = 10.00m,
                    OperatingHours = "07:00-22:00",
                    IsActive = true
                });
            }
        }

        // Add 5 Pickleball fields, each with 3 small fields
        for (int i = 1; i <= 5; i++)
        {
            allFields.Add(new Field
            {
                BusinessId = defaultVenue.BusinessId,
                VenueId = defaultVenue.Id,
                FieldName = $"Pickleball Field {i}",
                FieldNumber = $"Field {i}",
                SportType = SportType.Pickleball,
                Description = $"Main pickleball field {i}",
                BasePrice = 35.00m,
                OperatingHours = "07:00-22:00",
                IsActive = true
            });

            // Add 3 small fields for each main field
            foreach (char sub in new[] { 'A', 'B', 'C' })
            {
                allFields.Add(new Field
                {
                    BusinessId = defaultVenue.BusinessId,
                    VenueId = defaultVenue.Id,
                    FieldName = $"Pickleball Field {i} - Small Field {sub}",
                    FieldNumber = $"Field {i}{sub}",
                    SportType = SportType.Pickleball,
                    Description = $"Small pickleball field {sub} in Field {i}",
                    BasePrice = 12.00m,
                    OperatingHours = "07:00-22:00",
                    IsActive = true
                });
            }
        }

        foreach (var field in allFields)
        {
            if (!existingFieldNames.Contains(field.FieldName))
            {
                fieldsToAdd.Add(field);
            }
        }

        if (fieldsToAdd.Any())
        {
            _context.Fields.AddRange(fieldsToAdd);
            await _context.SaveChangesAsync();
            _logger.LogInformation("✅ Seeded {Count} new fields", fieldsToAdd.Count);
        }
        else
        {
            _logger.LogInformation("🏟️ All fields already exist, skipping...");
        }
    }
}
