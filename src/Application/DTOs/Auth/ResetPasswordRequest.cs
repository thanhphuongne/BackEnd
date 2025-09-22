namespace BackEnd.Application.DTOs.Auth;

public class ResetPasswordRequest
{
    public string Email { get; set; } = null!;
    public string ResetCode { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
}