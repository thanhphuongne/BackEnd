using System.ComponentModel.DataAnnotations;
using BackEnd.Domain.Enums;

namespace BackEnd.Application.DTOs.Auth;

public class RegisterRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = null!;

    [Required]
    [Compare("Password")]
    public string ConfirmPassword { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string FullName { get; set; } = null!;

    [StringLength(100)]
    public string? DisplayName { get; set; }

    [Phone]
    public string? Phone { get; set; }

    public DateTime? DateOfBirth { get; set; }

    [Required]
    public AccountType AccountType { get; set; } = AccountType.Customer;
}
