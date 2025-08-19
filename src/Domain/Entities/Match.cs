using BackEnd.Domain.Enums;

namespace BackEnd.Domain.Entities;

public class Match : BaseAuditableEntity
{
    public int OrganizerId { get; set; }
    
    public int FieldId { get; set; }
    
    public SportType SportType { get; set; }
    
    public DateTime StartTime { get; set; }
    
    public DateTime EndTime { get; set; }
    
    public SkillLevel SkillLevel { get; set; }
    
    public int MaxPlayers { get; set; }
    
    public int CurrentPlayers { get; set; } = 0;
    
    public string? Description { get; set; }
    
    public MatchPrivacy Privacy { get; set; }
    
    public MatchStatus Status { get; set; }
    
    // Navigation properties
    public User Organizer { get; set; } = null!;
    public Field Field { get; set; } = null!;
    public ICollection<MatchParticipant> Participants { get; set; } = new List<MatchParticipant>();
    public ICollection<MatchChat> Chats { get; set; } = new List<MatchChat>();
}
