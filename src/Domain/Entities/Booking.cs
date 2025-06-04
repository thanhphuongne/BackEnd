namespace BackEnd.Domain.Entities;

public class Booking : BaseAuditableEntity
{
    public int CustomerId { get; set; }
    
    public int FieldId { get; set; }
    
    public int SportId { get; set; }
    
    public DateTime BookingDate { get; set; }
    
    public TimeSpan StartTime { get; set; }
    
    public TimeSpan EndTime { get; set; }
    
    public decimal TotalPrice { get; set; }
    
    public string Status { get; set; } = "Pending";
    
    // Navigation properties
    public User Customer { get; set; } = null!;
    
    public Field Field { get; set; } = null!;
    
    public Sport Sport { get; set; } = null!;
}
