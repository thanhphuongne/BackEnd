namespace BackEnd.Domain.Entities;

public class Venue : BaseAuditableEntity
{
    public int BusinessId { get; set; }
    
    public string VenueName { get; set; } = null!;
    
    public string Address { get; set; } = null!;
    
    public string? Description { get; set; }
    
    public string? Photos { get; set; }
    
    public string? ContactPhone { get; set; }
    
    public string? ContactEmail { get; set; }
    
    public string? OperatingHours { get; set; }
    
    public string? Facilities { get; set; } // Parking, Restrooms, Cafeteria, etc.
    
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public Business Business { get; set; } = null!;
    
    public ICollection<Field> Fields { get; set; } = new List<Field>();
}
