using BackEnd.Application.Common.Exceptions;
using BackEnd.Application.Common.Interfaces;
using BackEnd.Domain.Entities;

namespace BackEnd.Application.Fields.Queries.GetFieldAvailability;

public record GetFieldAvailabilityQuery : IRequest<FieldAvailabilityDto>
{
    public int FieldId { get; init; }
    public DateTime Date { get; init; }
}

public class GetFieldAvailabilityQueryHandler : IRequestHandler<GetFieldAvailabilityQuery, FieldAvailabilityDto>
{
    private readonly IApplicationDbContext _context;

    public GetFieldAvailabilityQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FieldAvailabilityDto> Handle(GetFieldAvailabilityQuery request, CancellationToken cancellationToken)
    {
        var field = await _context.Fields
            .Include(f => f.Sport)
            .Include(f => f.Pricings)
            .FirstOrDefaultAsync(f => f.Id == request.FieldId, cancellationToken);

        if (field == null)
        {
            throw new NotFoundException(nameof(Field), request.FieldId);
        }

        var dayOfWeek = request.Date.DayOfWeek;
        
        // Get pricing for this day
        var pricing = field.Pricings
            .Where(p => p.DayOfWeek == dayOfWeek)
            .OrderBy(p => p.StartTime)
            .ToList();

        // Get existing bookings for this date
        var existingBookings = await _context.Bookings
            .Where(b => b.FieldId == request.FieldId 
                       && b.BookingDate.Date == request.Date.Date 
                       && b.Status != "Cancelled")
            .OrderBy(b => b.StartTime)
            .ToListAsync(cancellationToken);

        // Generate time slots
        var timeSlots = new List<TimeSlotDto>();
        
        foreach (var price in pricing)
        {
            var currentTime = price.StartTime;
            while (currentTime < price.EndTime)
            {
                var nextTime = currentTime.Add(TimeSpan.FromHours(1)); // 1-hour slots
                if (nextTime > price.EndTime) break;

                var isBooked = existingBookings.Any(b => 
                    (currentTime >= b.StartTime && currentTime < b.EndTime) ||
                    (nextTime > b.StartTime && nextTime <= b.EndTime) ||
                    (currentTime <= b.StartTime && nextTime >= b.EndTime));

                timeSlots.Add(new TimeSlotDto
                {
                    StartTime = currentTime,
                    EndTime = nextTime,
                    PricePerHour = price.PricePerHour,
                    IsAvailable = !isBooked
                });

                currentTime = nextTime;
            }
        }

        return new FieldAvailabilityDto
        {
            FieldId = field.Id,
            FieldName = field.FieldName,
            SportName = field.Sport.SportName,
            Date = request.Date,
            TimeSlots = timeSlots
        };
    }
}
