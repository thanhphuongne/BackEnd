using BackEnd.Domain.Enums;

namespace BackEnd.Domain.Entities;

public class FieldAvailability : BaseAuditableEntity
{
    public int FieldId { get; set; }
    
    public DateTime StartTime { get; set; }
    
    public DateTime EndTime { get; set; }
    
    public AvailabilityStatus Status { get; set; }
    
    // Navigation properties
    public Field Field { get; set; } = null!;
}
