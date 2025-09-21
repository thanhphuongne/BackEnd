namespace BackEnd.Application.DTOs.Match;

public class CreateMatchRequest
{
    public string Title { get; set; } = null!;
    public string Sport { get; set; } = null!;
    public string Date { get; set; } = null!;
    public string Time { get; set; } = null!;
    public string Location { get; set; } = null!;
    public string Address { get; set; } = null!;
    public int MaxParticipants { get; set; }
    public string SkillLevel { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Facebook { get; set; } = null!;
}