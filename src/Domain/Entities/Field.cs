namespace BackEnd.Domain.Entities;

public class Field
{
    public int FieldID { get; set; }
    public int SportID { get; set; }
    public string FieldName { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    
    // Navigation properties
    public Sport Sport { get; set; } = null!;
    public ICollection<Pricing> Pricings { get; set; } = new List<Pricing>();
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}