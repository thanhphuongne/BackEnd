using BackEnd.Domain.Enums;

namespace BackEnd.Domain.Entities;

public class PaymentMethod : BaseAuditableEntity
{
    public int UserId { get; set; }
    
    public PaymentMethodType MethodType { get; set; }
    
    public string? LastFourDigits { get; set; }
    
    public bool IsDefault { get; set; } = false;
    
    public string? BillingAddress { get; set; }
    
    // Navigation properties
    public User User { get; set; } = null!;
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
