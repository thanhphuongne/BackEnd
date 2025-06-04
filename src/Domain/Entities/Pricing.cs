namespace BackEnd.Domain.Entities;

public class Pricing : BaseAuditableEntity
{
    public int SportId { get; set; }
    
    public int FieldId { get; set; }
    
    public DayOfWeek DayOfWeek { get; set; }
    
    public TimeSpan StartTime { get; set; }
    
    public TimeSpan EndTime { get; set; }
    
    public decimal PricePerHour { get; set; }
    
    // Navigation properties
    public Sport Sport { get; set; } = null!;
    
    public Field Field { get; set; } = null!;
}
