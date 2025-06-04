namespace BackEnd.Domain.Entities;

public class Pricing
{
    public int PricingID { get; set; }
    public int SportID { get; set; }
    public int FieldID { get; set; }
    public string DayOfWeek { get; set; } = string.Empty;
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public decimal PricePerHour { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    
    // Navigation properties
    public Sport Sport { get; set; } = null!;
    public Field Field { get; set; } = null!;
}