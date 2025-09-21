using BackEnd.Application.DTOs.Booking;

namespace BackEnd.Application.Interfaces;

public interface IAnalyticsService
{
    Task<AnalyticsDto> GetAnalyticsAsync();
}