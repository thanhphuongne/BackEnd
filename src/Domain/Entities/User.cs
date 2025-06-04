namespace BackEnd.Domain.Entities;

public class User : BaseAuditableEntity
{
    public string UserName { get; set; } = null!;
    
    public string Password { get; set; } = null!;
    
    public string? Phone { get; set; }
    
    public string Email { get; set; } = null!;
    
    public string Role { get; set; } = null!;
    
    // Navigation properties
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
