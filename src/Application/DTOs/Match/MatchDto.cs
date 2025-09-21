namespace BackEnd.Application.DTOs.Match;

public class MatchDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string SportType { get; set; } = null!;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Location { get; set; } = null!;
    public string Address { get; set; } = null!;
    public int MaxParticipants { get; set; }
    public int CurrentPlayers { get; set; }
    public string SkillLevel { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string Privacy { get; set; } = null!;
    public string OrganizerName { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Facebook { get; set; } = null!;
    public DateTime Created { get; set; }
}