namespace BackEnd.Domain.Entities;

public class UserPreference : BaseAuditableEntity
{
    public int UserId { get; set; }
    
    public string? FavoriteSports { get; set; } // JSON string
    
    public string? PreferredLocations { get; set; } // JSON string
    
    public string? NotificationSettings { get; set; } // JSON string
    
    public string? PrivacySettings { get; set; } // JSON string
    
    // Navigation properties
    public User User { get; set; } = null!;
}
