namespace BackEnd.Application.DTOs.Auth;

public class AuthResponse
{
    public string Token { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public UserProfileDto User { get; set; } = null!;
}
