namespace BackEnd.Domain.Entities;

public class TeamMatch : BaseAuditableEntity
{
    public int CreatorId { get; set; }
    
    public int? OpponentId { get; set; }
    
    public int SportId { get; set; }
    
    public int FieldId { get; set; }
    
    public DateTime MatchDate { get; set; }
    
    public TimeSpan StartTime { get; set; }
    
    public TimeSpan EndTime { get; set; }
    
    public int PlayersNeeded { get; set; }
    
    public string SkillLevel { get; set; } = null!; // Beginner, Intermediate, Advanced, Mixed
    
    public string Status { get; set; } = "Looking"; // Looking, Matched, Confirmed, Cancelled, Completed
    
    public string? Description { get; set; }
    
    public string? ContactInfo { get; set; }
    
    public DateTime? MatchedAt { get; set; }
    
    public DateTime? ConfirmedAt { get; set; }
    
    // Navigation properties
    public User Creator { get; set; } = null!;
    
    public User? Opponent { get; set; }
    
    public Sport Sport { get; set; } = null!;
    
    public Field Field { get; set; } = null!;
    
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
