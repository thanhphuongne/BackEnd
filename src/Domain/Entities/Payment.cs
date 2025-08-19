using BackEnd.Domain.Enums;

namespace BackEnd.Domain.Entities;

public class Payment : BaseAuditableEntity
{
    public int BookingId { get; set; }
    
    public int UserId { get; set; }
    
    public int PaymentMethodId { get; set; }
    
    public decimal Amount { get; set; }
    
    public PaymentStatus Status { get; set; }
    
    public string? TransactionId { get; set; }
    
    // Navigation properties
    public Booking Booking { get; set; } = null!;
    public User User { get; set; } = null!;
    public PaymentMethod PaymentMethod { get; set; } = null!;
}
