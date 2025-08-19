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
}
