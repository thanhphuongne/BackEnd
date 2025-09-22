using BackEnd.Application.Common.Interfaces;
using BackEnd.Application.DTOs.Booking;
using BackEnd.Application.DTOs.Owner;
using BackEnd.Application.Interfaces;
using BackEnd.Domain.Entities;
using BackEnd.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Infrastructure.Services;

public class OwnerService : IOwnerService
{
    private readonly IApplicationDbContext _context;
    private readonly IAnalyticsService _analyticsService;

    public OwnerService(IApplicationDbContext context, IAnalyticsService analyticsService)
    {
        _context = context;
        _analyticsService = analyticsService;
    }

    public async Task<List<FieldDto>> GetMyFieldsAsync(string ownerId)
    {
        var owner = await _context.Users
            .Include(u => u.Business)
            .FirstOrDefaultAsync(u => u.Id.ToString() == ownerId);

        if (owner?.Business == null)
            return new List<FieldDto>();

        var fields = await _context.Fields
            .Where(f => f.BusinessId == owner.Business.Id)
            .Include(f => f.Venue)
            .ToListAsync();

        return fields.Select(f => new FieldDto
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
            BusinessName = owner.Business.BusinessName,
            VenueName = f.Venue.VenueName,
            VenueAddress = f.Venue.Address
        }).ToList();
    }

    public async Task<FieldDto?> GetFieldDetailsAsync(int fieldId, string ownerId)
    {
        var owner = await _context.Users
            .Include(u => u.Business)
            .FirstOrDefaultAsync(u => u.Id.ToString() == ownerId);

        if (owner?.Business == null)
            return null;

        var field = await _context.Fields
            .Where(f => f.Id == fieldId && f.BusinessId == owner.Business.Id)
            .Include(f => f.Venue)
            .FirstOrDefaultAsync();

        if (field == null)
            return null;

        return new FieldDto
        {
            Id = field.Id,
            BusinessId = field.BusinessId,
            VenueId = field.VenueId,
            FieldName = field.FieldName,
            FieldNumber = field.FieldNumber,
            SportType = field.SportType.ToString(),
            Address = field.Address,
            Description = field.Description,
            Photos = field.Photos,
            OperatingHours = field.OperatingHours,
            BasePrice = field.BasePrice,
            IsActive = field.IsActive,
            BusinessName = owner.Business.BusinessName,
            VenueName = field.Venue.VenueName,
            VenueAddress = field.Venue.Address
        };
    }

    public async Task<FieldDto> CreateFieldAsync(string ownerId, CreateFieldRequest request)
    {
        var owner = await _context.Users
            .Include(u => u.Business)
            .FirstOrDefaultAsync(u => u.Id.ToString() == ownerId);

        if (owner?.Business == null)
            throw new InvalidOperationException("Owner not found or has no business");

        // Create venue if needed or use existing
        var venue = await _context.Venues
            .FirstOrDefaultAsync(v => v.BusinessId == owner.Business.Id);

        if (venue == null)
        {
            venue = new Venue
            {
                BusinessId = owner.Business.Id,
                VenueName = $"{owner.FullName}'s Venue",
                Address = request.Location ?? "Default Address",
                IsActive = true
            };
            _context.Venues.Add(venue);
            await _context.SaveChangesAsync(default);
        }

        // Parse sport type
        if (!Enum.TryParse<SportType>(request.Sport, true, out var sportType))
            sportType = SportType.Soccer;

        var field = new Field
        {
            BusinessId = owner.Business.Id,
            VenueId = venue.Id,
            FieldName = request.Name,
            FieldNumber = "Field 1", // Default
            SportType = sportType,
            Address = request.Location,
            Description = request.Description,
            BasePrice = request.HourlyRate,
            IsActive = true
        };

        _context.Fields.Add(field);
        await _context.SaveChangesAsync(default);

        return new FieldDto
        {
            Id = field.Id,
            BusinessId = field.BusinessId,
            VenueId = field.VenueId,
            FieldName = field.FieldName,
            FieldNumber = field.FieldNumber,
            SportType = field.SportType.ToString(),
            Address = field.Address,
            Description = field.Description,
            Photos = field.Photos,
            OperatingHours = field.OperatingHours,
            BasePrice = field.BasePrice,
            IsActive = field.IsActive,
            BusinessName = owner.Business.BusinessName,
            VenueName = venue.VenueName,
            VenueAddress = venue.Address
        };
    }

    public async Task<bool> UpdateFieldAsync(int fieldId, string ownerId, UpdateFieldRequest request)
    {
        var owner = await _context.Users
            .Include(u => u.Business)
            .FirstOrDefaultAsync(u => u.Id.ToString() == ownerId);

        if (owner?.Business == null)
            return false;

        var field = await _context.Fields
            .FirstOrDefaultAsync(f => f.Id == fieldId && f.BusinessId == owner.Business.Id);

        if (field == null)
            return false;

        if (!string.IsNullOrEmpty(request.Name))
            field.FieldName = request.Name;

        if (request.HourlyRate.HasValue)
            field.BasePrice = request.HourlyRate.Value;

        await _context.SaveChangesAsync(default);
        return true;
    }

    public async Task<bool> DeleteFieldAsync(int fieldId, string ownerId)
    {
        var owner = await _context.Users
            .Include(u => u.Business)
            .FirstOrDefaultAsync(u => u.Id.ToString() == ownerId);

        if (owner?.Business == null)
            return false;

        var field = await _context.Fields
            .FirstOrDefaultAsync(f => f.Id == fieldId && f.BusinessId == owner.Business.Id);

        if (field == null)
            return false;

        _context.Fields.Remove(field);
        await _context.SaveChangesAsync(default);
        return true;
    }

    public async Task<List<BookingDto>> GetMyBookingsAsync(string ownerId)
    {
        var owner = await _context.Users
            .Include(u => u.Business)
            .FirstOrDefaultAsync(u => u.Id.ToString() == ownerId);

        if (owner?.Business == null)
            return new List<BookingDto>();

        var bookings = await _context.Bookings
            .Where(b => b.Field.BusinessId == owner.Business.Id)
            .Include(b => b.User)
            .Include(b => b.Field)
            .ToListAsync();

        return bookings.Select(b => new BookingDto
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
            FieldAddress = b.Field.Address ?? b.Field.Venue.Address
        }).ToList();
    }

    public async Task<BookingDto?> GetBookingDetailsAsync(int bookingId, string ownerId)
    {
        var owner = await _context.Users
            .Include(u => u.Business)
            .FirstOrDefaultAsync(u => u.Id.ToString() == ownerId);

        if (owner?.Business == null)
            return null;

        var booking = await _context.Bookings
            .Where(b => b.Id == bookingId && b.Field.BusinessId == owner.Business.Id)
            .Include(b => b.User)
            .Include(b => b.Field)
            .FirstOrDefaultAsync();

        if (booking == null)
            return null;

        return new BookingDto
        {
            Id = booking.Id,
            UserId = booking.UserId,
            FieldId = booking.FieldId,
            StartTime = booking.StartTime,
            EndTime = booking.EndTime,
            Status = booking.Status.ToString(),
            TotalPrice = booking.TotalPrice,
            PaymentStatus = booking.PaymentStatus.ToString(),
            Created = booking.Created.DateTime,
            UserName = booking.User.FullName,
            FieldName = booking.Field.FieldName,
            FieldAddress = booking.Field.Address ?? booking.Field.Venue.Address
        };
    }

    public async Task<bool> UpdateBookingStatusAsync(int bookingId, string ownerId, UpdateBookingStatusRequest request)
    {
        var owner = await _context.Users
            .Include(u => u.Business)
            .FirstOrDefaultAsync(u => u.Id.ToString() == ownerId);

        if (owner?.Business == null)
            return false;

        var booking = await _context.Bookings
            .FirstOrDefaultAsync(b => b.Id == bookingId && b.Field.BusinessId == owner.Business.Id);

        if (booking == null)
            return false;

        if (Enum.TryParse<BookingStatus>(request.Status, true, out var status))
        {
            booking.Status = status;
            booking.LastModified = DateTime.UtcNow;
            await _context.SaveChangesAsync(default);
            return true;
        }

        return false;
    }

    public async Task<bool> CancelBookingAsync(int bookingId, string ownerId)
    {
        var owner = await _context.Users
            .Include(u => u.Business)
            .FirstOrDefaultAsync(u => u.Id.ToString() == ownerId);

        if (owner?.Business == null)
            return false;

        var booking = await _context.Bookings
            .FirstOrDefaultAsync(b => b.Id == bookingId && b.Field.BusinessId == owner.Business.Id);

        if (booking == null)
            return false;

        booking.Status = BookingStatus.Cancelled;
        booking.LastModified = DateTime.UtcNow;
        await _context.SaveChangesAsync(default);
        return true;
    }

    public async Task<AnalyticsDto> GetAnalyticsAsync(string ownerId)
    {
        // For now, return general analytics - in a real implementation,
        // this would filter by owner's fields
        return await _analyticsService.GetAnalyticsAsync();
    }

    public async Task<PaymentSettingsDto?> GetPaymentSettingsAsync(string ownerId)
    {
        var owner = await _context.Users
            .Include(u => u.Business)
            .FirstOrDefaultAsync(u => u.Id.ToString() == ownerId);

        if (owner?.Business == null)
            return null;

        // This would typically be stored in a separate table
        // For now, return default settings
        return new PaymentSettingsDto
        {
            Commission = 5.0m,
            AcceptedMethods = new List<string> { "cash", "bank_transfer" },
            BankDetails = new BankDetailsDto
            {
                AccountName = owner.FullName,
                AccountNumber = "123456789",
                BankName = "Default Bank"
            }
        };
    }

    public async Task<bool> UpdatePaymentSettingsAsync(string ownerId, PaymentSettingsDto settings)
    {
        // Implementation would save to database
        // For now, just return success
        return await Task.FromResult(true);
    }

    public async Task<string?> UploadAvatarAsync(string ownerId, Stream avatarStream, string fileName)
    {
        // Implementation would save file and return URL
        // For now, return a mock URL
        return await Task.FromResult($"https://example.com/avatars/{ownerId}/{fileName}");
    }
}