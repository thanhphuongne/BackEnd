namespace BackEnd.Application.DTOs.Booking;

public class SportDto
{
    public int Id { get; set; }
    public string SportName { get; set; } = null!;
    public string? Description { get; set; }
}

public class VenueDto
{
    public int Id { get; set; }
    public int BusinessId { get; set; }
    public string VenueName { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string? Description { get; set; }
    public string? Photos { get; set; }
    public string? ContactPhone { get; set; }
    public string? ContactEmail { get; set; }
    public string? OperatingHours { get; set; }
    public string? Facilities { get; set; }
    public bool IsActive { get; set; }
    public string? BusinessName { get; set; }
    public int FieldCount { get; set; }
    public List<string> AvailableSports { get; set; } = new();
}

public class FieldDto
{
    public int Id { get; set; }
    public int BusinessId { get; set; }
    public int VenueId { get; set; }
    public string FieldName { get; set; } = null!;
    public string FieldNumber { get; set; } = null!;
    public string SportType { get; set; } = null!;
    public string? Address { get; set; }
    public string? Description { get; set; }
    public string? Photos { get; set; }
    public string? OperatingHours { get; set; }
    public decimal BasePrice { get; set; }
    public bool IsActive { get; set; }
    public string? BusinessName { get; set; }
    public string? VenueName { get; set; }
    public string? VenueAddress { get; set; }
}

public class FieldAvailabilityDto
{
    public int Id { get; set; }
    public int FieldId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Status { get; set; } = null!;
    public decimal Price { get; set; }
}

public class BookingRequestDto
{
    public int FieldId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}

public class BookingDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int FieldId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Status { get; set; } = null!;
    public decimal TotalPrice { get; set; }
    public string PaymentStatus { get; set; } = null!;
    public DateTime Created { get; set; }
    
    // Navigation properties
    public string? UserName { get; set; }
    public string? FieldName { get; set; }
    public string? FieldAddress { get; set; }
}
