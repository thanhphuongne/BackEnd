using BackEnd.Application.Common.Interfaces;
using BackEnd.Application.DTOs.Booking;
using BackEnd.Domain.Entities;
using BackEnd.Domain.Enums;
using BackEnd.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackEnd.Infrastructure.Services;

public class BookingService : IBookingService
{
    private readonly IApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<BookingService> _logger;

    public BookingService(IApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<BookingService> logger)
    {
        _context = context;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<List<SportDto>> GetAllSportsAsync()
    {
        _logger.LogInformation("🏃 Getting all sports");

        // For now, return all sports (we can add field filtering later)
        var sports = await _context.Sports
            .Select(s => new SportDto
            {
                Id = s.Id,
                SportName = s.SportName,
                Description = s.Description
            })
            .ToListAsync();

        _logger.LogInformation("✅ Found {Count} sports", sports.Count);
        return sports;
    }

    public async Task<SportDto?> GetSportByIdAsync(int id)
    {
        _logger.LogInformation("🔍 Getting sport by ID: {SportId}", id);
        
        var sport = await _context.Sports
            .Where(s => s.Id == id)
            .Select(s => new SportDto
            {
                Id = s.Id,
                SportName = s.SportName,
                Description = s.Description
            })
            .FirstOrDefaultAsync();

        if (sport == null)
            _logger.LogWarning("❌ Sport not found: {SportId}", id);
        else
            _logger.LogInformation("✅ Found sport: {SportName}", sport.SportName);

        return sport;
    }

    public async Task<List<VenueDto>> GetAllVenuesAsync()
    {
        _logger.LogInformation("🏟️ Getting all venues");

        var venues = await _context.Venues
            .Include(v => v.Business)
            .Include(v => v.Fields)
            .Where(v => v.IsActive && v.Fields.Any(f => f.IsActive))
            .Select(v => new VenueDto
            {
                Id = v.Id,
                BusinessId = v.BusinessId,
                VenueName = v.VenueName,
                Address = v.Address,
                Description = v.Description,
                Photos = v.Photos,
                ContactPhone = v.ContactPhone,
                ContactEmail = v.ContactEmail,
                OperatingHours = v.OperatingHours,
                Facilities = v.Facilities,
                IsActive = v.IsActive,
                BusinessName = v.Business.BusinessName,
                FieldCount = v.Fields.Count(f => f.IsActive),
                AvailableSports = v.Fields.Where(f => f.IsActive).Select(f => f.SportType.ToString()).Distinct().ToList()
            })
            .ToListAsync();

        _logger.LogInformation("✅ Found {Count} venues", venues.Count);
        return venues;
    }

    public async Task<List<VenueDto>> GetVenuesBySportAsync(int sportId)
    {
        _logger.LogInformation("🏟️ Getting venues for sport ID: {SportId}", sportId);

        var venues = await _context.Venues
            .Include(v => v.Business)
            .Include(v => v.Fields)
            .Where(v => v.IsActive && v.Fields.Any(f => f.IsActive && f.SportType == (SportType)sportId))
            .Select(v => new VenueDto
            {
                Id = v.Id,
                BusinessId = v.BusinessId,
                VenueName = v.VenueName,
                Address = v.Address,
                Description = v.Description,
                Photos = v.Photos,
                ContactPhone = v.ContactPhone,
                ContactEmail = v.ContactEmail,
                OperatingHours = v.OperatingHours,
                Facilities = v.Facilities,
                IsActive = v.IsActive,
                BusinessName = v.Business.BusinessName,
                FieldCount = v.Fields.Count(f => f.IsActive && f.SportType == (SportType)sportId),
                AvailableSports = v.Fields.Where(f => f.IsActive).Select(f => f.SportType.ToString()).Distinct().ToList()
            })
            .ToListAsync();

        _logger.LogInformation("✅ Found {Count} venues for sport ID: {SportId}", venues.Count, sportId);
        return venues;
    }

    public async Task<VenueDto?> GetVenueByIdAsync(int id)
    {
        _logger.LogInformation("🔍 Getting venue by ID: {VenueId}", id);

        var venue = await _context.Venues
            .Include(v => v.Business)
            .Include(v => v.Fields)
            .Where(v => v.Id == id && v.IsActive)
            .Select(v => new VenueDto
            {
                Id = v.Id,
                BusinessId = v.BusinessId,
                VenueName = v.VenueName,
                Address = v.Address,
                Description = v.Description,
                Photos = v.Photos,
                ContactPhone = v.ContactPhone,
                ContactEmail = v.ContactEmail,
                OperatingHours = v.OperatingHours,
                Facilities = v.Facilities,
                IsActive = v.IsActive,
                BusinessName = v.Business.BusinessName,
                FieldCount = v.Fields.Count(f => f.IsActive),
                AvailableSports = v.Fields.Where(f => f.IsActive).Select(f => f.SportType.ToString()).Distinct().ToList()
            })
            .FirstOrDefaultAsync();

        if (venue == null)
            _logger.LogWarning("❌ Venue not found: {VenueId}", id);
        else
            _logger.LogInformation("✅ Found venue: {VenueName}", venue.VenueName);

        return venue;
    }

    public async Task<List<FieldDto>> GetFieldsByVenueAsync(int venueId)
    {
        _logger.LogInformation("🏟️ Getting fields for venue ID: {VenueId}", venueId);

        var fields = await _context.Fields
            .Include(f => f.Business)
            .Include(f => f.Venue)
            .Where(f => f.VenueId == venueId && f.IsActive)
            .Select(f => new FieldDto
            {
                Id = f.Id,
                BusinessId = f.BusinessId,
                VenueId = f.VenueId,
                FieldName = f.FieldName,
                FieldNumber = f.FieldNumber,
                SportType = f.SportType.ToString(),
                Address = f.Address,
                Description = f.Description,
                Photos = f.Photos,
                OperatingHours = f.OperatingHours,
                BasePrice = f.BasePrice,
                IsActive = f.IsActive,
                BusinessName = f.Business.BusinessName,
                VenueName = f.Venue.VenueName,
                VenueAddress = f.Venue.Address
            })
            .ToListAsync();

        _logger.LogInformation("✅ Found {Count} fields for venue ID: {VenueId}", fields.Count, venueId);
        return fields;
    }

    public async Task<List<FieldDto>> GetFieldsBySportAsync(int sportId)
    {
        _logger.LogInformation("🏟️ Getting fields for sport ID: {SportId}", sportId);

        var fields = await _context.Fields
            .Include(f => f.Business)
            .Include(f => f.Venue)
            .Where(f => f.SportType == (SportType)sportId && f.IsActive)
            .Select(f => new FieldDto
            {
                Id = f.Id,
                BusinessId = f.BusinessId,
                VenueId = f.VenueId,
                FieldName = f.FieldName,
                FieldNumber = f.FieldNumber,
                SportType = f.SportType.ToString(),
                Address = f.Address,
                Description = f.Description,
                Photos = f.Photos,
                OperatingHours = f.OperatingHours,
                BasePrice = f.BasePrice,
                IsActive = f.IsActive,
                BusinessName = f.Business.BusinessName,
                VenueName = f.Venue.VenueName,
                VenueAddress = f.Venue.Address
            })
            .ToListAsync();

        _logger.LogInformation("✅ Found {Count} fields for sport ID: {SportId}", fields.Count, sportId);
        return fields;
    }

    public async Task<List<FieldDto>> GetFieldsBySportNameAsync(string sportName)
    {
        _logger.LogInformation("🏟️ Getting fields for sport name: {SportName}", sportName);

        if (!Enum.TryParse<SportType>(sportName, true, out var sportType))
        {
            _logger.LogWarning("❌ Invalid sport name: {SportName}", sportName);
            return new List<FieldDto>();
        }

        var fields = await _context.Fields
            .Include(f => f.Business)
            .Include(f => f.Venue)
            .Where(f => f.SportType == sportType && f.IsActive)
            .Select(f => new FieldDto
            {
                Id = f.Id,
                BusinessId = f.BusinessId,
                VenueId = f.VenueId,
                FieldName = f.FieldName,
                FieldNumber = f.FieldNumber,
                SportType = f.SportType.ToString(),
                Address = f.Address,
                Description = f.Description,
                Photos = f.Photos,
                OperatingHours = f.OperatingHours,
                BasePrice = f.BasePrice,
                IsActive = f.IsActive,
                BusinessName = f.Business.BusinessName,
                VenueName = f.Venue.VenueName,
                VenueAddress = f.Venue.Address
            })
            .ToListAsync();

        _logger.LogInformation("✅ Found {Count} fields for sport name: {SportName}", fields.Count, sportName);
        return fields;
    }

    public async Task<List<FieldDto>> GetPopularFieldsAsync()
    {
        _logger.LogInformation("🏟️ Getting popular fields");

        var fields = await _context.Fields
            .Include(f => f.Business)
            .Include(f => f.Venue)
            .Where(f => f.IsActive)
            .OrderByDescending(f => f.Bookings.Count(b => b.Status != BookingStatus.Cancelled))
            .Take(10)
            .Select(f => new FieldDto
            {
                Id = f.Id,
                BusinessId = f.BusinessId,
                VenueId = f.VenueId,
                FieldName = f.FieldName,
                FieldNumber = f.FieldNumber,
                SportType = f.SportType.ToString(),
                Address = f.Address,
                Description = f.Description,
                Photos = f.Photos,
                OperatingHours = f.OperatingHours,
                BasePrice = f.BasePrice,
                IsActive = f.IsActive,
                BusinessName = f.Business.BusinessName,
                VenueName = f.Venue.VenueName,
                VenueAddress = f.Venue.Address
            })
            .ToListAsync();

        _logger.LogInformation("✅ Found {Count} popular fields", fields.Count);
        return fields;
    }

    public async Task<List<FieldDto>> GetAllFieldsAsync()
    {
        _logger.LogInformation("🏟️ Getting all active fields");
        
        var fields = await _context.Fields
            .Include(f => f.Business)
            .Include(f => f.Venue)
            .Where(f => f.IsActive)
            .Select(f => new FieldDto
            {
                Id = f.Id,
                BusinessId = f.BusinessId,
                VenueId = f.VenueId,
                FieldName = f.FieldName,
                FieldNumber = f.FieldNumber,
                SportType = f.SportType.ToString(),
                Address = f.Address,
                Description = f.Description,
                Photos = f.Photos,
                OperatingHours = f.OperatingHours,
                BasePrice = f.BasePrice,
                IsActive = f.IsActive,
                BusinessName = f.Business.BusinessName,
                VenueName = f.Venue.VenueName,
                VenueAddress = f.Venue.Address
            })
            .ToListAsync();

        _logger.LogInformation("✅ Found {Count} active fields", fields.Count);
        return fields;
    }

    public async Task<FieldDto?> GetFieldByIdAsync(int id)
    {
        _logger.LogInformation("🔍 Getting field by ID: {FieldId}", id);
        
        var field = await _context.Fields
            .Include(f => f.Business)
            .Include(f => f.Venue)
            .Where(f => f.Id == id && f.IsActive)
            .Select(f => new FieldDto
            {
                Id = f.Id,
                BusinessId = f.BusinessId,
                VenueId = f.VenueId,
                FieldName = f.FieldName,
                FieldNumber = f.FieldNumber,
                SportType = f.SportType.ToString(),
                Address = f.Address,
                Description = f.Description,
                Photos = f.Photos,
                OperatingHours = f.OperatingHours,
                BasePrice = f.BasePrice,
                IsActive = f.IsActive,
                BusinessName = f.Business.BusinessName,
                VenueName = f.Venue.VenueName,
                VenueAddress = f.Venue.Address
            })
            .FirstOrDefaultAsync();

        if (field == null)
            _logger.LogWarning("❌ Field not found or inactive: {FieldId}", id);
        else
            _logger.LogInformation("✅ Found field: {FieldName}", field.FieldName);

        return field;
    }

    public async Task<List<FieldAvailabilityDto>> GetFieldAvailabilityAsync(int fieldId, DateTime date)
    {
        _logger.LogInformation("📅 Getting availability for field {FieldId} on {Date}", fieldId, date.ToString("yyyy-MM-dd"));
        
        var startOfDay = date.Date;
        var endOfDay = startOfDay.AddDays(1);

        // Get existing bookings for the day
        var existingBookings = await _context.Bookings
            .Where(b => b.FieldId == fieldId && 
                       b.StartTime >= startOfDay && 
                       b.StartTime < endOfDay &&
                       b.Status != BookingStatus.Cancelled)
            .ToListAsync();

        // Generate hourly slots (8 AM to 10 PM)
        var availableSlots = new List<FieldAvailabilityDto>();
        var field = await _context.Fields.FindAsync(fieldId);
        
        if (field == null)
        {
            _logger.LogWarning("❌ Field not found: {FieldId}", fieldId);
            return availableSlots;
        }

        for (int hour = 8; hour < 22; hour++)
        {
            var slotStart = startOfDay.AddHours(hour);
            var slotEnd = slotStart.AddHours(1);
            
            var isBooked = existingBookings.Any(b => 
                (b.StartTime < slotEnd && b.EndTime > slotStart));

            var price = await CalculateBookingPriceAsync(fieldId, slotStart, slotEnd);

            availableSlots.Add(new FieldAvailabilityDto
            {
                Id = 0, // Generated slot
                FieldId = fieldId,
                StartTime = slotStart,
                EndTime = slotEnd,
                Status = isBooked ? AvailabilityStatus.Booked.ToString() : AvailabilityStatus.Available.ToString(),
                Price = price
            });
        }

        _logger.LogInformation("✅ Generated {Count} time slots for field {FieldId}", availableSlots.Count, fieldId);
        return availableSlots;
    }

    public async Task<List<FieldAvailabilityDto>> GetFieldAvailabilityRangeAsync(int fieldId, DateTime startDate, DateTime endDate)
    {
        _logger.LogInformation("📅 Getting availability for field {FieldId} from {StartDate} to {EndDate}", 
            fieldId, startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
        
        var allSlots = new List<FieldAvailabilityDto>();
        var currentDate = startDate.Date;
        
        while (currentDate <= endDate.Date)
        {
            var daySlots = await GetFieldAvailabilityAsync(fieldId, currentDate);
            allSlots.AddRange(daySlots);
            currentDate = currentDate.AddDays(1);
        }

        _logger.LogInformation("✅ Generated {Count} total time slots for field {FieldId}", allSlots.Count, fieldId);
        return allSlots;
    }

    public async Task<decimal> CalculateBookingPriceAsync(int fieldId, DateTime startTime, DateTime endTime)
    {
        var field = await _context.Fields.FindAsync(fieldId);
        if (field == null) return 0;

        var duration = endTime - startTime;
        var hours = (decimal)duration.TotalHours;
        
        // Base price calculation
        var basePrice = field.BasePrice * hours;
        
        // Apply pricing rules (if any)
        var pricingRules = await _context.PricingRules
            .Where(pr => pr.FieldId == fieldId)
            .ToListAsync();

        foreach (var rule in pricingRules)
        {
            // Apply time-based pricing
            if (rule.StartTime.HasValue && rule.EndTime.HasValue)
            {
                var ruleStart = TimeSpan.FromTicks(rule.StartTime.Value.Ticks);
                var ruleEnd = TimeSpan.FromTicks(rule.EndTime.Value.Ticks);
                var bookingTime = startTime.TimeOfDay;
                
                if (bookingTime >= ruleStart && bookingTime < ruleEnd)
                {
                    if (rule.PriceModifier.HasValue)
                    {
                        basePrice *= rule.PriceModifier.Value;
                    }
                }
            }
            
            // Apply day-of-week pricing
            if (rule.DayOfWeek.HasValue)
            {
                var dayOfWeek = (WeekDay)((int)startTime.DayOfWeek);
                if (rule.DayOfWeek == dayOfWeek && rule.PriceModifier.HasValue)
                {
                    basePrice *= rule.PriceModifier.Value;
                }
            }
        }

        return Math.Round(basePrice, 2);
    }

    public async Task<BookingDto?> CreateBookingAsync(int userId, BookingRequestDto request)
    {
        _logger.LogInformation("📝 Creating booking for user {UserId}, field {FieldId}, time {StartTime}-{EndTime}",
            userId, request.FieldId, request.StartTime, request.EndTime);

        // Validate field exists and is active
        var field = await _context.Fields
            .Include(f => f.Business)
            .FirstOrDefaultAsync(f => f.Id == request.FieldId && f.IsActive);

        if (field == null)
        {
            _logger.LogWarning("❌ Field not found or inactive: {FieldId}", request.FieldId);
            return null;
        }

        // Check if time slot is available
        var existingBooking = await _context.Bookings
            .AnyAsync(b => b.FieldId == request.FieldId &&
                          b.Status != BookingStatus.Cancelled &&
                          ((b.StartTime < request.EndTime && b.EndTime > request.StartTime)));

        if (existingBooking)
        {
            _logger.LogWarning("❌ Time slot already booked for field {FieldId}", request.FieldId);
            return null;
        }

        // Calculate total price
        var totalPrice = await CalculateBookingPriceAsync(request.FieldId, request.StartTime, request.EndTime);

        // Create booking
        var booking = new Booking
        {
            UserId = userId,
            FieldId = request.FieldId,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            Status = BookingStatus.Confirmed,
            TotalPrice = totalPrice,
            PaymentStatus = PaymentStatus.Pending
        };

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync(CancellationToken.None);

        _logger.LogInformation("✅ Booking created successfully: ID {BookingId}, Price: ${TotalPrice}",
            booking.Id, totalPrice);

        return await GetBookingByIdAsync(booking.Id);
    }

    public async Task<List<BookingDto>> GetUserBookingsAsync(int userId)
    {
        _logger.LogInformation("📋 Getting bookings for user {UserId}", userId);

        var bookings = await _context.Bookings
            .Include(b => b.Field)
            .Include(b => b.User)
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.Created)
            .Select(b => new BookingDto
            {
                Id = b.Id,
                UserId = b.UserId,
                FieldId = b.FieldId,
                StartTime = b.StartTime,
                EndTime = b.EndTime,
                Status = b.Status.ToString(),
                TotalPrice = b.TotalPrice,
                PaymentStatus = b.PaymentStatus.ToString(),
                Created = b.Created.DateTime,
                UserName = b.User.FullName,
                FieldName = b.Field.FieldName,
                FieldAddress = b.Field.Address
            })
            .ToListAsync();

        _logger.LogInformation("✅ Found {Count} bookings for user {UserId}", bookings.Count, userId);
        return bookings;
    }

    public async Task<BookingDto?> GetBookingByIdAsync(int bookingId)
    {
        _logger.LogInformation("🔍 Getting booking by ID: {BookingId}", bookingId);

        var booking = await _context.Bookings
            .Include(b => b.Field)
            .Include(b => b.User)
            .Where(b => b.Id == bookingId)
            .Select(b => new BookingDto
            {
                Id = b.Id,
                UserId = b.UserId,
                FieldId = b.FieldId,
                StartTime = b.StartTime,
                EndTime = b.EndTime,
                Status = b.Status.ToString(),
                TotalPrice = b.TotalPrice,
                PaymentStatus = b.PaymentStatus.ToString(),
                Created = b.Created.DateTime,
                UserName = b.User.FullName,
                FieldName = b.Field.FieldName,
                FieldAddress = b.Field.Address
            })
            .FirstOrDefaultAsync();

        if (booking == null)
            _logger.LogWarning("❌ Booking not found: {BookingId}", bookingId);
        else
            _logger.LogInformation("✅ Found booking: {FieldName} on {StartTime}", booking.FieldName, booking.StartTime);

        return booking;
    }

    public async Task<bool> CancelBookingAsync(int bookingId, int userId)
    {
        _logger.LogInformation("❌ Cancelling booking {BookingId} for user {UserId}", bookingId, userId);

        var booking = await _context.Bookings
            .FirstOrDefaultAsync(b => b.Id == bookingId && b.UserId == userId);

        if (booking == null)
        {
            _logger.LogWarning("❌ Booking not found or user not authorized: {BookingId}", bookingId);
            return false;
        }

        if (booking.Status == BookingStatus.Cancelled)
        {
            _logger.LogWarning("❌ Booking already cancelled: {BookingId}", bookingId);
            return false;
        }

        // Check if booking can be cancelled (e.g., not too close to start time)
        if (booking.StartTime <= DateTime.UtcNow.AddHours(2))
        {
            _logger.LogWarning("❌ Cannot cancel booking less than 2 hours before start time: {BookingId}", bookingId);
            return false;
        }

        booking.Status = BookingStatus.Cancelled;
        await _context.SaveChangesAsync(CancellationToken.None);

        _logger.LogInformation("✅ Booking cancelled successfully: {BookingId}", bookingId);
        return true;
    }

    public async Task<Domain.Entities.User?> GetUserByIdentityIdAsync(string identityId)
    {
        _logger.LogInformation("🔍 Looking up user by Identity ID: {IdentityId}", identityId);

        // First get the Identity user to get the email
        var identityUser = await _userManager.FindByIdAsync(identityId);

        if (identityUser == null)
        {
            _logger.LogWarning("❌ Identity user not found: {IdentityId}", identityId);
            return null;
        }

        // Then get our business user by email
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == identityUser.Email);

        if (user == null)
        {
            _logger.LogWarning("❌ Business user not found for email: {Email}", identityUser.Email);
            return null;
        }

        _logger.LogInformation("✅ Found user: {UserName} ({Email})", user.FullName, user.Email);
        return user;
    }
}