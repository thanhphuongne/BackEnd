namespace BackEnd.Application.DTOs.Owner;

public class UpdateBookingStatusRequest
{
    public string Status { get; set; } = null!;
    public string? Notes { get; set; }
}