using BackEnd.Domain.Enums;

namespace BackEnd.Domain.Entities;

public class Field : BaseAuditableEntity
{
    public int BusinessId { get; set; }

    public int VenueId { get; set; }

    public string FieldName { get; set; } = null!;

    public string FieldNumber { get; set; } = null!; // "Field 1", "Court A", etc.

    public SportType SportType { get; set; }

    public string? Address { get; set; } // Optional, can inherit from venue

    public string? Description { get; set; }

    public string? Photos { get; set; } // JSON string

    public string? OperatingHours { get; set; } // JSON string

    public decimal BasePrice { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation properties
    public Business Business { get; set; } = null!;
    public Venue Venue { get; set; } = null!;
    public ICollection<FieldAvailability> FieldAvailabilities { get; set; } = new List<FieldAvailability>();
    public ICollection<PricingRule> PricingRules { get; set; } = new List<PricingRule>();
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public ICollection<Match> Matches { get; set; } = new List<Match>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
