using BackEnd.Domain.Enums;

namespace BackEnd.Domain.Entities;

public class User : BaseAuditableEntity
{
    public string Email { get; set; } = null!;
    
    public string? Phone { get; set; }
    
    public string FullName { get; set; } = null!;
    
    public string? DisplayName { get; set; }
    
    public string PasswordHash { get; set; } = null!;
    
    public DateTime? DateOfBirth { get; set; }
    
    public string? ProfilePhotoUrl { get; set; }
    
    public AccountType AccountType { get; set; }
    
    public string? VerificationCode { get; set; }
    
    public DateTime? LastLogin { get; set; }
    
    // Navigation properties
    public Business? Business { get; set; }
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public ICollection<PaymentMethod> PaymentMethods { get; set; } = new List<PaymentMethod>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public ICollection<Match> OrganizedMatches { get; set; } = new List<Match>();
    public ICollection<MatchParticipant> MatchParticipations { get; set; } = new List<MatchParticipant>();
    public ICollection<MatchChat> MatchChats { get; set; } = new List<MatchChat>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public UserPreference? UserPreference { get; set; }
}
