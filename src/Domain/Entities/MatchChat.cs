namespace BackEnd.Domain.Entities;

public class MatchChat : BaseAuditableEntity
{
    public int MatchId { get; set; }
    
    public int UserId { get; set; }
    
    public string MessageText { get; set; } = null!;
    
    public DateTime SentAt { get; set; }
    
    // Navigation properties
    public Match Match { get; set; } = null!;
    public User User { get; set; } = null!;
}
