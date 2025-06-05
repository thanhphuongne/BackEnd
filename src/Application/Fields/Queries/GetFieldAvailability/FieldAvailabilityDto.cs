namespace BackEnd.Application.Fields.Queries.GetFieldAvailability;

public class FieldAvailabilityDto
{
    public int FieldId { get; init; }
    public string FieldName { get; init; } = null!;
    public string SportName { get; init; } = null!;
    public DateTime Date { get; init; }
    public IList<TimeSlotDto> TimeSlots { get; init; } = Array.Empty<TimeSlotDto>();
}

public class TimeSlotDto
{
    public TimeSpan StartTime { get; init; }
    public TimeSpan EndTime { get; init; }
    public decimal PricePerHour { get; init; }
    public bool IsAvailable { get; init; }
}
