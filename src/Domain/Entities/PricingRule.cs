using BackEnd.Domain.Enums;

namespace BackEnd.Domain.Entities;

public class PricingRule : BaseAuditableEntity
{
    public int FieldId { get; set; }
    
    public TimeSpan? StartTime { get; set; }
    
    public TimeSpan? EndTime { get; set; }
    
    public WeekDay? DayOfWeek { get; set; }
    
    public decimal? PriceModifier { get; set; }
    
    public bool IsHoliday { get; set; } = false;
    
    // Navigation properties
    public Field Field { get; set; } = null!;
}
