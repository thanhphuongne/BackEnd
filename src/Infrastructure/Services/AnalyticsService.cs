using BackEnd.Application.Common.Interfaces;
using BackEnd.Application.DTOs.Booking;
using BackEnd.Application.Interfaces;
using BackEnd.Domain.Entities;
using BackEnd.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Infrastructure.Services;

public class AnalyticsService : IAnalyticsService
{
    private readonly IApplicationDbContext _context;

    public AnalyticsService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AnalyticsDto> GetAnalyticsAsync()
    {
        var now = DateTimeOffset.UtcNow;
        var startOfMonth = new DateTimeOffset(now.Year, now.Month, 1, 0, 0, 0, TimeSpan.Zero);
        var startOfDay = new DateTimeOffset(now.Date, TimeSpan.Zero);

        var bookings = await _context.Bookings
            .Include(b => b.Field)
            .Include(b => b.User)
            .ToListAsync();

        var completedBookings = bookings.Where(b => b.Status == BookingStatus.Completed).ToList();
        var allBookings = bookings.Where(b => b.Status != BookingStatus.Pending).ToList();
        var cancelledBookings = bookings.Where(b => b.Status == BookingStatus.Cancelled).ToList();

        var monthlyBookings = allBookings.Where(b => b.Created >= startOfMonth).ToList();
        var dailyBookings = allBookings.Where(b => b.Created >= startOfDay).ToList();

        var analytics = new AnalyticsDto
        {
            TotalRevenue = completedBookings.Sum(b => b.TotalPrice),
            MonthlyRevenue = completedBookings.Where(b => b.Created >= startOfMonth).Sum(b => b.TotalPrice),
            DailyRevenue = completedBookings.Where(b => b.Created >= startOfDay).Sum(b => b.TotalPrice),
            TotalBookings = allBookings.Count,
            MonthlyBookings = monthlyBookings.Count,
            DailyBookings = dailyBookings.Count,
            AverageBookingValue = allBookings.Any() ? allBookings.Average(b => b.TotalPrice) : 0,
            PopularFields = GetPopularFields(completedBookings),
            PeakHours = GetPeakHours(allBookings),
            CustomerRetention = CalculateCustomerRetention(bookings),
            CancellationRate = allBookings.Any() ? (decimal)cancelledBookings.Count / allBookings.Count * 100 : 0
        };

        return analytics;
    }

    private List<PopularFieldDto> GetPopularFields(List<Booking> bookings)
    {
        return bookings
            .GroupBy(b => b.Field.FieldName)
            .Select(g => new PopularFieldDto
            {
                FieldName = g.Key,
                Bookings = g.Count(),
                Revenue = g.Sum(b => b.TotalPrice)
            })
            .OrderByDescending(p => p.Bookings)
            .Take(10)
            .ToList();
    }

    private List<PeakHourDto> GetPeakHours(List<Booking> bookings)
    {
        return bookings
            .GroupBy(b => b.StartTime.Hour)
            .Select(g => new PeakHourDto
            {
                Hour = $"{g.Key}:00",
                Bookings = g.Count()
            })
            .OrderByDescending(p => p.Bookings)
            .Take(10)
            .ToList();
    }

    private decimal CalculateCustomerRetention(List<Booking> bookings)
    {
        var usersWithMultipleBookings = bookings
            .GroupBy(b => b.UserId)
            .Count(g => g.Count() > 1);

        var totalUsers = bookings.Select(b => b.UserId).Distinct().Count();

        return totalUsers > 0 ? (decimal)usersWithMultipleBookings / totalUsers * 100 : 0;
    }
}