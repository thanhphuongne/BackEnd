namespace BackEnd.Domain.Entities;

public class Booking : BaseAuditableEntity
{
    public int CustomerId { get; set; }

    public int FieldId { get; set; }

    public int SportId { get; set; }

    public DateTime BookingDate { get; set; }

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }

    public decimal TotalPrice { get; set; }

    public string Status { get; set; } = "Pending"; // Pending, Confirmed, Cancelled, Completed

    public string? Notes { get; set; }

    public int? TeamMatchId { get; set; }

    public string? EquipmentRented { get; set; } // JSON array of equipment IDs and quantities

    public decimal? EquipmentCost { get; set; }

    public DateTime? CancelledAt { get; set; }

    public string? CancellationReason { get; set; }

    public bool IsRecurring { get; set; } = false;

    public string? RecurrencePattern { get; set; } // Weekly, Monthly, etc.

    // Navigation properties
    public User Customer { get; set; } = null!;

    public Field Field { get; set; } = null!;

    public Sport Sport { get; set; } = null!;

    public TeamMatch? TeamMatch { get; set; }

    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
