namespace BackEnd.Application.DTOs.Auth;

public class UserProfileDto
{
    public string Id { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string? DisplayName { get; set; }
    public string? Phone { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? ProfilePhotoUrl { get; set; }
    public string AccountType { get; set; } = null!;
    public DateTime? LastLogin { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Bio { get; set; }
    public string? Country { get; set; }
    public string? CityState { get; set; }
    public string? PostalCode { get; set; }
    public string? Facebook { get; set; }
    public string? Twitter { get; set; }
    public string? Linkedin { get; set; }
    public string? Instagram { get; set; }
}
