using System.ComponentModel.DataAnnotations;

namespace BackEnd.Application.DTOs.Auth;

public class LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = null!;

    public bool RememberMe { get; set; } = false;
}
