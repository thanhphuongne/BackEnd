namespace BackEnd.Domain.Entities;

public class Review : BaseAuditableEntity
{
    public int BookingId { get; set; }
    
    public int UserId { get; set; }
    
    public int FieldId { get; set; }
    
    public int OverallRating { get; set; } // 1-5
    
    public int FieldConditionRating { get; set; } // 1-5
    
    public int FacilitiesRating { get; set; } // 1-5
    
    public int StaffRating { get; set; } // 1-5
    
    public int ValueRating { get; set; } // 1-5
    
    public string? ReviewText { get; set; }
    
    public string? Photos { get; set; } // JSON string
    
    // Navigation properties
    public Booking Booking { get; set; } = null!;
    public User User { get; set; } = null!;
    public Field Field { get; set; } = null!;
}
