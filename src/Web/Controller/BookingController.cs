using BackEnd.Application.Common.Interfaces;
using BackEnd.Application.DTOs.Booking;
using BackEnd.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BackEnd.Web.Controller;

[ApiController]
[Route("api/[controller]")]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;
    private readonly IAnalyticsService _analyticsService;
    private readonly ILogger<BookingController> _logger;

    public BookingController(IBookingService bookingService, IAnalyticsService analyticsService, ILogger<BookingController> logger)
    {
        _bookingService = bookingService;
        _analyticsService = analyticsService;
        _logger = logger;
    }

    /// <summary>
    /// Get all available sports
    /// </summary>
    [HttpGet("sports")]
    public async Task<IActionResult> GetSports()
    {
        _logger.LogInformation("🏃 Getting all sports");
        var sports = await _bookingService.GetAllSportsAsync();
        return Ok(sports);
    }

    /// <summary>
    /// Get sport by ID
    /// </summary>
    [HttpGet("sports/{id}")]
    public async Task<IActionResult> GetSport(int id)
    {
        _logger.LogInformation("🔍 Getting sport by ID: {SportId}", id);
        var sport = await _bookingService.GetSportByIdAsync(id);
        
        if (sport == null)
            return NotFound(new { message = "Sport not found" });
            
        return Ok(sport);
    }

    /// <summary>
    /// Get all venues or venues by sport
    /// </summary>
    [HttpGet("venues")]
    public async Task<IActionResult> GetVenues([FromQuery] int? sportId)
    {
        if (sportId.HasValue)
        {
            _logger.LogInformation("🏟️ Getting venues for sport: {SportId}", sportId.Value);
            var venues = await _bookingService.GetVenuesBySportAsync(sportId.Value);
            return Ok(venues);
        }
        else
        {
            _logger.LogInformation("🏟️ Getting all venues");
            var venues = await _bookingService.GetAllVenuesAsync();
            return Ok(venues);
        }
    }

    /// <summary>
    /// Get venue by ID
    /// </summary>
    [HttpGet("venues/{id}")]
    public async Task<IActionResult> GetVenue(int id)
    {
        _logger.LogInformation("🔍 Getting venue by ID: {VenueId}", id);
        var venue = await _bookingService.GetVenueByIdAsync(id);

        if (venue == null)
            return NotFound(new { message = "Venue not found" });

        return Ok(venue);
    }

    /// <summary>
    /// Get fields by venue
    /// </summary>
    [HttpGet("venues/{venueId}/fields")]
    public async Task<IActionResult> GetFieldsByVenue(int venueId)
    {
        _logger.LogInformation("🏟️ Getting fields for venue: {VenueId}", venueId);
        var fields = await _bookingService.GetFieldsByVenueAsync(venueId);
        return Ok(fields);
    }

    /// <summary>
    /// Get all fields or fields by sport
    /// </summary>
    [HttpGet("fields")]
    public async Task<IActionResult> GetFields([FromQuery] int? sportId)
    {
        if (sportId.HasValue)
        {
            _logger.LogInformation("🏟️ Getting fields for sport: {SportId}", sportId.Value);
            var fields = await _bookingService.GetFieldsBySportAsync(sportId.Value);
            return Ok(fields);
        }
        else
        {
            _logger.LogInformation("🏟️ Getting all fields");
            var fields = await _bookingService.GetAllFieldsAsync();
            return Ok(fields);
        }
    }

    /// <summary>
    /// Get field by ID
    /// </summary>
    [HttpGet("fields/{id}")]
    public async Task<IActionResult> GetField(int id)
    {
        _logger.LogInformation("🔍 Getting field by ID: {FieldId}", id);
        var field = await _bookingService.GetFieldByIdAsync(id);
        
        if (field == null)
            return NotFound(new { message = "Field not found" });
            
        return Ok(field);
    }

    /// <summary>
    /// Get field availability for a specific date
    /// </summary>
    [HttpGet("fields/{fieldId}/availability")]
    public async Task<IActionResult> GetFieldAvailability(int fieldId, [FromQuery] DateTime date)
    {
        _logger.LogInformation("📅 Getting availability for field {FieldId} on {Date}", fieldId, date.ToString("yyyy-MM-dd"));
        
        if (date.Date < DateTime.Today)
        {
            return BadRequest(new { message = "Cannot get availability for past dates" });
        }
        
        var availability = await _bookingService.GetFieldAvailabilityAsync(fieldId, date);
        return Ok(availability);
    }

    /// <summary>
    /// Get field availability for a date range
    /// </summary>
    [HttpGet("fields/{fieldId}/availability/range")]
    public async Task<IActionResult> GetFieldAvailabilityRange(int fieldId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        _logger.LogInformation("📅 Getting availability for field {FieldId} from {StartDate} to {EndDate}", 
            fieldId, startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
        
        if (startDate.Date < DateTime.Today)
        {
            return BadRequest(new { message = "Start date cannot be in the past" });
        }
        
        if (endDate < startDate)
        {
            return BadRequest(new { message = "End date must be after start date" });
        }
        
        if ((endDate - startDate).Days > 30)
        {
            return BadRequest(new { message = "Date range cannot exceed 30 days" });
        }
        
        var availability = await _bookingService.GetFieldAvailabilityRangeAsync(fieldId, startDate, endDate);
        return Ok(availability);
    }

    /// <summary>
    /// Calculate booking price
    /// </summary>
    [HttpGet("fields/{fieldId}/price")]
    public async Task<IActionResult> CalculatePrice(int fieldId, [FromQuery] DateTime startTime, [FromQuery] DateTime endTime)
    {
        _logger.LogInformation("💰 Calculating price for field {FieldId}, time {StartTime}-{EndTime}", 
            fieldId, startTime, endTime);
        
        if (startTime >= endTime)
        {
            return BadRequest(new { message = "End time must be after start time" });
        }
        
        var price = await _bookingService.CalculateBookingPriceAsync(fieldId, startTime, endTime);
        return Ok(new { fieldId, startTime, endTime, price });
    }

    /// <summary>
    /// Create a new booking (requires authentication)
    /// </summary>
    [HttpPost("bookings")]
    [Authorize]
    public async Task<IActionResult> CreateBooking([FromBody] BookingRequestDto request)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        
        if (string.IsNullOrEmpty(userIdClaim))
        {
            _logger.LogWarning("❌ No user ID in token");
            return Unauthorized();
        }

        // Convert Identity user ID to our User entity ID
        var user = await _bookingService.GetUserByIdentityIdAsync(userIdClaim);
        if (user == null)
        {
            _logger.LogWarning("❌ User not found for Identity ID: {IdentityId}", userIdClaim);
            return Unauthorized();
        }

        _logger.LogInformation("📝 Creating booking for user: {Email} (ID: {UserId})", userEmail, user.Id);
        
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("❌ Invalid booking request from user: {Email}", userEmail);
            return BadRequest(ModelState);
        }
        
        if (request.StartTime <= DateTime.UtcNow)
        {
            return BadRequest(new { message = "Booking time must be in the future" });
        }
        
        if (request.StartTime >= request.EndTime)
        {
            return BadRequest(new { message = "End time must be after start time" });
        }
        
        var booking = await _bookingService.CreateBookingAsync(user.Id, request);
        if (booking == null)
        {
            return BadRequest(new { message = "Unable to create booking. Time slot may be unavailable." });
        }
        
        _logger.LogInformation("✅ Booking created successfully for user: {Email}, Booking ID: {BookingId}", 
            userEmail, booking.Id);
        return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, booking);
    }

    /// <summary>
    /// Get user's bookings (requires authentication)
    /// </summary>
    [HttpGet("bookings/my")]
    [Authorize]
    public async Task<IActionResult> GetMyBookings()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        
        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized();

        var user = await _bookingService.GetUserByIdentityIdAsync(userIdClaim);
        if (user == null)
            return Unauthorized();

        _logger.LogInformation("📋 Getting bookings for user: {Email}", userEmail);
        
        var bookings = await _bookingService.GetUserBookingsAsync(user.Id);
        return Ok(bookings);
    }

    /// <summary>
    /// Get booking by ID
    /// </summary>
    [HttpGet("bookings/{id}")]
    [Authorize]
    public async Task<IActionResult> GetBooking(int id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized();

        var user = await _bookingService.GetUserByIdentityIdAsync(userIdClaim);
        if (user == null)
            return Unauthorized();

        _logger.LogInformation("🔍 Getting booking {BookingId} for user: {UserId}", id, user.Id);
        
        var booking = await _bookingService.GetBookingByIdAsync(id);
        if (booking == null)
            return NotFound(new { message = "Booking not found" });
            
        // Ensure user can only see their own bookings
        if (booking.UserId != user.Id)
            return Forbid();
            
        return Ok(booking);
    }

    /// <summary>
    /// Cancel a booking (requires authentication)
    /// </summary>
    [HttpPost("bookings/{id}/cancel")]
    [Authorize]
    public async Task<IActionResult> CancelBooking(int id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        
        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized();

        var user = await _bookingService.GetUserByIdentityIdAsync(userIdClaim);
        if (user == null)
            return Unauthorized();

        _logger.LogInformation("❌ Cancelling booking {BookingId} for user: {Email}", id, userEmail);
        
        var result = await _bookingService.CancelBookingAsync(id, user.Id);
        if (!result)
        {
            return BadRequest(new { message = "Unable to cancel booking. It may not exist, already be cancelled, or be too close to the start time." });
        }
        
        _logger.LogInformation("✅ Booking {BookingId} cancelled successfully by user: {Email}", id, userEmail);
        return Ok(new { message = "Booking cancelled successfully" });
    }

    /// <summary>
    /// Get analytics data
    /// </summary>
    [HttpGet("analytics")]
    [Authorize]
    public async Task<IActionResult> GetAnalytics()
    {
        _logger.LogInformation("📊 Getting analytics data");
        var analytics = await _analyticsService.GetAnalyticsAsync();
        return Ok(analytics);
    }
}
