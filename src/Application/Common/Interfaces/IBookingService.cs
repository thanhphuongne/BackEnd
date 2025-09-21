using BackEnd.Application.DTOs.Booking;

namespace BackEnd.Application.Common.Interfaces;

public interface IBookingService
{
    // Sports
    Task<List<SportDto>> GetAllSportsAsync();
    Task<SportDto?> GetSportByIdAsync(int id);

    // Venues
    Task<List<VenueDto>> GetVenuesBySportAsync(int sportId);
    Task<List<VenueDto>> GetAllVenuesAsync();
    Task<VenueDto?> GetVenueByIdAsync(int id);

    // Fields
    Task<List<FieldDto>> GetFieldsByVenueAsync(int venueId);
    Task<List<FieldDto>> GetFieldsBySportAsync(int sportId);
    Task<List<FieldDto>> GetFieldsBySportNameAsync(string sportName);
    Task<List<FieldDto>> GetPopularFieldsAsync();
    Task<List<FieldDto>> GetAllFieldsAsync();
    Task<FieldDto?> GetFieldByIdAsync(int id);
    
    // Field Availability
    Task<List<FieldAvailabilityDto>> GetFieldAvailabilityAsync(int fieldId, DateTime date);
    Task<List<FieldAvailabilityDto>> GetFieldAvailabilityRangeAsync(int fieldId, DateTime startDate, DateTime endDate);
    
    // Bookings
    Task<BookingDto?> CreateBookingAsync(int userId, BookingRequestDto request);
    Task<List<BookingDto>> GetUserBookingsAsync(int userId);
    Task<BookingDto?> GetBookingByIdAsync(int bookingId);
    Task<bool> CancelBookingAsync(int bookingId, int userId);
    
    // Pricing
    Task<decimal> CalculateBookingPriceAsync(int fieldId, DateTime startTime, DateTime endTime);

    // User lookup
    Task<Domain.Entities.User?> GetUserByIdentityIdAsync(string identityId);
}
