using BackEnd.Application.DTOs.Booking;
using BackEnd.Application.DTOs.Owner;

namespace BackEnd.Application.Interfaces;

public interface IOwnerService
{
    // Field Management
    Task<List<FieldDto>> GetMyFieldsAsync(string ownerId);
    Task<FieldDto?> GetFieldDetailsAsync(int fieldId, string ownerId);
    Task<FieldDto> CreateFieldAsync(string ownerId, CreateFieldRequest request);
    Task<bool> UpdateFieldAsync(int fieldId, string ownerId, UpdateFieldRequest request);
    Task<bool> DeleteFieldAsync(int fieldId, string ownerId);

    // Booking Management
    Task<List<BookingDto>> GetMyBookingsAsync(string ownerId);
    Task<BookingDto?> GetBookingDetailsAsync(int bookingId, string ownerId);
    Task<bool> UpdateBookingStatusAsync(int bookingId, string ownerId, UpdateBookingStatusRequest request);
    Task<bool> CancelBookingAsync(int bookingId, string ownerId);

    // Analytics
    Task<AnalyticsDto> GetAnalyticsAsync(string ownerId);

    // Payment Settings
    Task<PaymentSettingsDto?> GetPaymentSettingsAsync(string ownerId);
    Task<bool> UpdatePaymentSettingsAsync(string ownerId, PaymentSettingsDto settings);

    // Avatar Upload
    Task<string?> UploadAvatarAsync(string ownerId, Stream avatarStream, string fileName);
}