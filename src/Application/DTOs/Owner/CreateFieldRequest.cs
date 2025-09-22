namespace BackEnd.Application.DTOs.Owner;

public class CreateFieldRequest
{
    public string Name { get; set; } = null!;
    public string? Sport { get; set; }
    public string? Description { get; set; }
    public string? Location { get; set; }
    public decimal HourlyRate { get; set; }
    public decimal? PeakRate { get; set; }
    public string? OpenTime { get; set; }
    public string? CloseTime { get; set; }
    public int Capacity { get; set; }
    public List<string>? Amenities { get; set; }
    public List<string>? Images { get; set; }
}