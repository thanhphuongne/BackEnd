namespace BackEnd.Domain.Entities;

public class Payment : BaseAuditableEntity
{
    public int BookingId { get; set; }
    
    public int CustomerId { get; set; }
    
    public decimal Amount { get; set; }
    
    public string PaymentMethod { get; set; } = null!; // CreditCard, BankTransfer, MoMo, VNPay, Cash
    
    public string Status { get; set; } = "Pending"; // Pending, Completed, Failed, Refunded
    
    public string? TransactionId { get; set; }
    
    public string? PaymentGatewayResponse { get; set; }
    
    public DateTime? PaidAt { get; set; }
    
    public DateTime? RefundedAt { get; set; }
    
    public decimal? RefundAmount { get; set; }
    
    public string? RefundReason { get; set; }
    
    public string? InvoiceNumber { get; set; }
    
    public string? Notes { get; set; }
    
    // Navigation properties
    public Booking Booking { get; set; } = null!;
    
    public User Customer { get; set; } = null!;
}
