using BackEnd.Application.DTOs.Booking;
using BackEnd.Application.DTOs.Owner;
using BackEnd.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BackEnd.Web.Controller;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OwnerController : ControllerBase
{
    private readonly IOwnerService _ownerService;
    private readonly ILogger<OwnerController> _logger;

    public OwnerController(IOwnerService ownerService, ILogger<OwnerController> logger)
    {
        _ownerService = ownerService;
        _logger = logger;
    }

    /// <summary>
    /// Get all fields owned by the current user
    /// </summary>
    [HttpGet("fields")]
    public async Task<IActionResult> GetMyFields()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        _logger.LogInformation("🏟️ Getting fields for owner: {UserId}", userId);
        var fields = await _ownerService.GetMyFieldsAsync(userId);
        return Ok(fields);
    }

    /// <summary>
    /// Get field details by ID
    /// </summary>
    [HttpGet("fields/{id}")]
    public async Task<IActionResult> GetField(int id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        _logger.LogInformation("🔍 Getting field {FieldId} for owner: {UserId}", id, userId);
        var field = await _ownerService.GetFieldDetailsAsync(id, userId);

        if (field == null)
            return NotFound(new { message = "Field not found or access denied" });

        return Ok(field);
    }

    /// <summary>
    /// Create a new field
    /// </summary>
    [HttpPost("fields")]
    public async Task<IActionResult> CreateField([FromBody] CreateFieldRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _logger.LogInformation("➕ Creating field for owner: {UserId}", userId);
        var field = await _ownerService.CreateFieldAsync(userId, request);
        return CreatedAtAction(nameof(GetField), new { id = field.Id }, field);
    }

    /// <summary>
    /// Update field details
    /// </summary>
    [HttpPut("fields/{id}")]
    public async Task<IActionResult> UpdateField(int id, [FromBody] UpdateFieldRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _logger.LogInformation("✏️ Updating field {FieldId} for owner: {UserId}", id, userId);
        var result = await _ownerService.UpdateFieldAsync(id, userId, request);

        if (!result)
            return NotFound(new { message = "Field not found or access denied" });

        return Ok(new { message = "Field updated successfully" });
    }

    /// <summary>
    /// Delete a field
    /// </summary>
    [HttpDelete("fields/{id}")]
    public async Task<IActionResult> DeleteField(int id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        _logger.LogInformation("🗑️ Deleting field {FieldId} for owner: {UserId}", id, userId);
        var result = await _ownerService.DeleteFieldAsync(id, userId);

        if (!result)
            return NotFound(new { message = "Field not found or access denied" });

        return Ok(new { message = "Field deleted successfully" });
    }

    /// <summary>
    /// Get all bookings for owner's fields
    /// </summary>
    [HttpGet("bookings")]
    public async Task<IActionResult> GetMyBookings()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        _logger.LogInformation("📋 Getting bookings for owner: {UserId}", userId);
        var bookings = await _ownerService.GetMyBookingsAsync(userId);
        return Ok(bookings);
    }

    /// <summary>
    /// Get booking details by ID
    /// </summary>
    [HttpGet("bookings/{id}")]
    public async Task<IActionResult> GetBooking(int id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        _logger.LogInformation("🔍 Getting booking {BookingId} for owner: {UserId}", id, userId);
        var booking = await _ownerService.GetBookingDetailsAsync(id, userId);

        if (booking == null)
            return NotFound(new { message = "Booking not found or access denied" });

        return Ok(booking);
    }

    /// <summary>
    /// Update booking status
    /// </summary>
    [HttpPut("bookings/{id}")]
    public async Task<IActionResult> UpdateBookingStatus(int id, [FromBody] UpdateBookingStatusRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _logger.LogInformation("📝 Updating booking {BookingId} status for owner: {UserId}", id, userId);
        var result = await _ownerService.UpdateBookingStatusAsync(id, userId, request);

        if (!result)
            return BadRequest(new { message = "Failed to update booking status" });

        return Ok(new { message = "Booking status updated successfully" });
    }

    /// <summary>
    /// Cancel a booking
    /// </summary>
    [HttpPut("bookings/{id}/cancel")]
    public async Task<IActionResult> CancelBooking(int id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        _logger.LogInformation("❌ Cancelling booking {BookingId} for owner: {UserId}", id, userId);
        var result = await _ownerService.CancelBookingAsync(id, userId);

        if (!result)
            return BadRequest(new { message = "Failed to cancel booking" });

        return Ok(new { message = "Booking cancelled successfully" });
    }

    /// <summary>
    /// Get analytics data for owner
    /// </summary>
    [HttpGet("analytics")]
    public async Task<IActionResult> GetAnalytics()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        _logger.LogInformation("📊 Getting analytics for owner: {UserId}", userId);
        var analytics = await _ownerService.GetAnalyticsAsync(userId);
        return Ok(analytics);
    }

    /// <summary>
    /// Get payment settings
    /// </summary>
    [HttpGet("payment-settings")]
    public async Task<IActionResult> GetPaymentSettings()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        _logger.LogInformation("💳 Getting payment settings for owner: {UserId}", userId);
        var settings = await _ownerService.GetPaymentSettingsAsync(userId);

        if (settings == null)
            return NotFound(new { message = "Payment settings not found" });

        return Ok(settings);
    }

    /// <summary>
    /// Update payment settings
    /// </summary>
    [HttpPut("payment-settings")]
    public async Task<IActionResult> UpdatePaymentSettings([FromBody] PaymentSettingsDto settings)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _logger.LogInformation("💳 Updating payment settings for owner: {UserId}", userId);
        var result = await _ownerService.UpdatePaymentSettingsAsync(userId, settings);

        if (!result)
            return BadRequest(new { message = "Failed to update payment settings" });

        return Ok(new { message = "Payment settings updated successfully" });
    }

    /// <summary>
    /// Upload avatar
    /// </summary>
    [HttpPost("upload-avatar")]
    public async Task<IActionResult> UploadAvatar(IFormFile avatar)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        if (avatar == null || avatar.Length == 0)
            return BadRequest(new { message = "No file uploaded" });

        _logger.LogInformation("📸 Uploading avatar for owner: {UserId}", userId);

        using var stream = avatar.OpenReadStream();
        var url = await _ownerService.UploadAvatarAsync(userId, stream, avatar.FileName);

        if (url == null)
            return BadRequest(new { message = "Failed to upload avatar" });

        return Ok(new { avatarUrl = url });
    }
}