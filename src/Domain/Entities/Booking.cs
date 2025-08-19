using BackEnd.Domain.Enums;

namespace BackEnd.Domain.Entities;

public class Booking : BaseAuditableEntity
{
    public int UserId { get; set; }

    public int FieldId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public BookingStatus Status { get; set; }

    public decimal TotalPrice { get; set; }

    public PaymentStatus PaymentStatus { get; set; }

    public DateTime? ModifiedAt { get; set; }

    // Navigation properties
    public User User { get; set; } = null!;
    public Field Field { get; set; } = null!;
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public Review? Review { get; set; }
}
