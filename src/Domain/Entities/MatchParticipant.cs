using BackEnd.Domain.Enums;

namespace BackEnd.Domain.Entities;

public class MatchParticipant : BaseAuditableEntity
{
    public int MatchId { get; set; }
    
    public int UserId { get; set; }
    
    public ParticipantStatus Status { get; set; }
    
    public DateTime JoinedAt { get; set; }
    
    // Navigation properties
    public Match Match { get; set; } = null!;
    public User User { get; set; } = null!;
}
