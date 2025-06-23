namespace BackEnd.Domain.Entities;

public class Field : BaseAuditableEntity
{
    public int SportId { get; set; }
    
    public string FieldName { get; set; } = null!;
    
    public string? Location { get; set; }
    
    public int? Capacity { get; set; }
    
    public string? Description { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public Sport Sport { get; set; } = null!;
    
    public ICollection<Pricing> Pricings { get; set; } = new List<Pricing>();
    
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
