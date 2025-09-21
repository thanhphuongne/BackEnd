namespace BackEnd.Application.DTOs.Booking;

public class AnalyticsDto
{
    public decimal TotalRevenue { get; set; }
    public decimal MonthlyRevenue { get; set; }
    public decimal DailyRevenue { get; set; }
    public int TotalBookings { get; set; }
    public int MonthlyBookings { get; set; }
    public int DailyBookings { get; set; }
    public decimal AverageBookingValue { get; set; }
    public List<PopularFieldDto> PopularFields { get; set; } = new();
    public List<PeakHourDto> PeakHours { get; set; } = new();
    public decimal CustomerRetention { get; set; }
    public decimal CancellationRate { get; set; }
}

public class PopularFieldDto
{
    public string FieldName { get; set; } = null!;
    public int Bookings { get; set; }
    public decimal Revenue { get; set; }
}

public class PeakHourDto
{
    public string Hour { get; set; } = null!;
    public int Bookings { get; set; }
}