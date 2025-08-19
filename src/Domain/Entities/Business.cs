namespace BackEnd.Domain.Entities;

public class Business : BaseAuditableEntity
{
    public int OwnerId { get; set; }
    
    public string BusinessName { get; set; } = null!;
    
    public string? ContactEmail { get; set; }
    
    public string? ContactPhone { get; set; }
    
    public string Address { get; set; } = null!;
    
    // Navigation properties
    public User Owner { get; set; } = null!;
    public ICollection<Field> Fields { get; set; } = new List<Field>();
}
