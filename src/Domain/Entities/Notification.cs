using BackEnd.Domain.Enums;

namespace BackEnd.Domain.Entities;

public class Notification : BaseAuditableEntity
{
    public int UserId { get; set; }
    
    public NotificationType Type { get; set; }
    
    public string Title { get; set; } = null!;
    
    public string Message { get; set; } = null!;
    
    public DeliveryMethod DeliveryMethod { get; set; }
    
    public NotificationStatus Status { get; set; }
    
    // Navigation properties
    public User User { get; set; } = null!;
}
