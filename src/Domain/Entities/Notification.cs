namespace BackEnd.Domain.Entities;

public class Notification : BaseAuditableEntity
{
    public int UserId { get; set; }
    
    public string Title { get; set; } = null!;
    
    public string Message { get; set; } = null!;
    
    public string Type { get; set; } = null!; // BookingConfirmed, TeamMatched, PaymentReceived, etc.
    
    public bool IsRead { get; set; } = false;
    
    public DateTime? ReadAt { get; set; }
    
    public string? RelatedEntityType { get; set; } // Booking, TeamMatch, Payment
    
    public int? RelatedEntityId { get; set; }
    
    public string? ActionUrl { get; set; }
    
    // Navigation properties
    public User User { get; set; } = null!;
}
