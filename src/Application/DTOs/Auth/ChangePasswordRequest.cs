using System.ComponentModel.DataAnnotations;

namespace BackEnd.Application.DTOs.Auth;

public class ChangePasswordRequest
{
    [Required]
    public string CurrentPassword { get; set; } = null!;

    [Required]
    [MinLength(6)]
    public string NewPassword { get; set; } = null!;

    [Required]
    [Compare("NewPassword")]
    public string ConfirmNewPassword { get; set; } = null!;
}
