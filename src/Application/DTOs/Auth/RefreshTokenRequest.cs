using System.ComponentModel.DataAnnotations;

namespace BackEnd.Application.DTOs.Auth;

public class RefreshTokenRequest
{
    [Required]
    public string RefreshToken { get; set; } = null!;
}
