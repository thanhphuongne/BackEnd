using Microsoft.AspNetCore.Identity;

namespace BackEnd.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public string? VerificationCode { get; set; }
}
