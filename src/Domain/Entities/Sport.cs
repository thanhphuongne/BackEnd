namespace BackEnd.Domain.Entities;

public class Sport : BaseAuditableEntity
{
    public string SportName { get; set; } = null!;
    
    public string? Description { get; set; }
    
    // Navigation properties
    public ICollection<Field> Fields { get; set; } = new List<Field>();
    
    public ICollection<Pricing> Pricings { get; set; } = new List<Pricing>();
    
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
