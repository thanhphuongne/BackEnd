namespace BackEnd.Domain.Entities;

public class User : BaseAuditableEntity
{
    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Phone { get; set; }

    public string Email { get; set; } = null!;

    public string Role { get; set; } = null!; // User, FieldOwner, Administrator

    public string? FullName { get; set; }

    public string? ProfileImageUrl { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? PreferredSports { get; set; } // JSON array of sport IDs

    public string? SkillLevel { get; set; } // Beginner, Intermediate, Advanced

    public bool IsEmailVerified { get; set; } = false;

    public bool IsPhoneVerified { get; set; } = false;

    public DateTime? LastLoginAt { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation properties
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public ICollection<Field> OwnedFields { get; set; } = new List<Field>();

    public ICollection<TeamMatch> TeamMatches { get; set; } = new List<TeamMatch>();

    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
