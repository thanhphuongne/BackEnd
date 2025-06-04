namespace BackEnd.Domain.Entities;

public class Booking
{
    public int BookingID { get; set; }
    public int CustomerID { get; set; }
    public int FieldID { get; set; }
    public int SportID { get; set; }
    public DateTime BookingDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    
    // Navigation properties
    public User Customer { get; set; } = null!;
    public Field Field { get; set; } = null!;
    public Sport Sport { get; set; } = null!;
}