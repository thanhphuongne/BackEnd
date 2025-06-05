using BackEnd.Application.Common.Interfaces;
using BackEnd.Domain.Entities;

namespace BackEnd.Application.Bookings.Commands.CreateBooking;

public record CreateBookingCommand : IRequest<int>
{
    public int CustomerId { get; init; }
    public int FieldId { get; init; }
    public int SportId { get; init; }
    public DateTime BookingDate { get; init; }
    public TimeSpan StartTime { get; init; }
    public TimeSpan EndTime { get; init; }
    public string? Notes { get; init; }
    public string? EquipmentRented { get; init; }
    public bool IsRecurring { get; init; }
    public string? RecurrencePattern { get; init; }
}

public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateBookingCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        // Calculate total price
        var totalPrice = await CalculateTotalPrice(request, cancellationToken);

        var entity = new Booking
        {
            CustomerId = request.CustomerId,
            FieldId = request.FieldId,
            SportId = request.SportId,
            BookingDate = request.BookingDate,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            TotalPrice = totalPrice.FieldCost,
            EquipmentCost = totalPrice.EquipmentCost,
            Notes = request.Notes,
            EquipmentRented = request.EquipmentRented,
            IsRecurring = request.IsRecurring,
            RecurrencePattern = request.RecurrencePattern,
            Status = "Pending"
        };

        _context.Bookings.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }

    private async Task<(decimal FieldCost, decimal EquipmentCost)> CalculateTotalPrice(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        var dayOfWeek = request.BookingDate.DayOfWeek;
        var duration = request.EndTime - request.StartTime;
        var hours = (decimal)duration.TotalHours;

        // Get field pricing
        var pricing = await _context.Pricings
            .Where(p => p.FieldId == request.FieldId 
                       && p.SportId == request.SportId
                       && p.DayOfWeek == dayOfWeek
                       && p.StartTime <= request.StartTime
                       && p.EndTime >= request.EndTime)
            .FirstOrDefaultAsync(cancellationToken);

        var fieldCost = pricing?.PricePerHour * hours ?? 0;

        // Calculate equipment cost if any
        var equipmentCost = 0m;
        if (!string.IsNullOrEmpty(request.EquipmentRented))
        {
            // Parse equipment JSON and calculate cost
            // This is a simplified version - in real implementation, you'd parse JSON
            equipmentCost = 0; // TODO: Implement equipment cost calculation
        }

        return (fieldCost, equipmentCost);
    }
}
