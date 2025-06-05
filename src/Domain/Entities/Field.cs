namespace BackEnd.Domain.Entities;

public class Field : BaseAuditableEntity
{
    public int SportId { get; set; }

    public string FieldName { get; set; } = null!;

    public string? Location { get; set; }

    public string? City { get; set; }

    public string? District { get; set; }

    public string? Address { get; set; }

    public int? Capacity { get; set; }

    public string? Description { get; set; }

    public string? FieldType { get; set; } // Indoor, Outdoor, 5-a-side, 7-a-side, etc.

    public bool HasLighting { get; set; } = false;

    public bool HasParking { get; set; } = false;

    public bool HasRestrooms { get; set; } = false;

    public bool HasWater { get; set; } = false;

    public bool HasSoundSystem { get; set; } = false;

    public string? ImageUrl { get; set; }

    public decimal? Rating { get; set; }

    public int ReviewCount { get; set; } = 0;

    public bool IsActive { get; set; } = true;

    public int OwnerId { get; set; }

    // Navigation properties
    public Sport Sport { get; set; } = null!;

    public User Owner { get; set; } = null!;

    public ICollection<Pricing> Pricings { get; set; } = new List<Pricing>();

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public ICollection<Equipment> Equipment { get; set; } = new List<Equipment>();
}
