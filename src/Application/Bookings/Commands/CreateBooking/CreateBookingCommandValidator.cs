using BackEnd.Application.Common.Interfaces;

namespace BackEnd.Application.Bookings.Commands.CreateBooking;

public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateBookingCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required.")
            .MustAsync(CustomerExists).WithMessage("The specified customer does not exist.");

        RuleFor(v => v.FieldId)
            .NotEmpty().WithMessage("Field ID is required.")
            .MustAsync(FieldExists).WithMessage("The specified field does not exist.");

        RuleFor(v => v.SportId)
            .NotEmpty().WithMessage("Sport ID is required.")
            .MustAsync(SportExists).WithMessage("The specified sport does not exist.");

        RuleFor(v => v.BookingDate)
            .NotEmpty().WithMessage("Booking date is required.")
            .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Booking date cannot be in the past.");

        RuleFor(v => v.StartTime)
            .NotEmpty().WithMessage("Start time is required.");

        RuleFor(v => v.EndTime)
            .NotEmpty().WithMessage("End time is required.")
            .GreaterThan(v => v.StartTime).WithMessage("End time must be after start time.");

        RuleFor(v => v)
            .MustAsync(BeAvailableTimeSlot).WithMessage("The selected time slot is not available.");

        RuleFor(v => v.Notes)
            .MaximumLength(1000).WithMessage("Notes must not exceed 1000 characters.");

        RuleFor(v => v.RecurrencePattern)
            .MaximumLength(100).WithMessage("Recurrence pattern must not exceed 100 characters.");
    }

    public async Task<bool> CustomerExists(int customerId, CancellationToken cancellationToken)
    {
        return await _context.CustomUsers
            .AnyAsync(u => u.Id == customerId, cancellationToken);
    }

    public async Task<bool> FieldExists(int fieldId, CancellationToken cancellationToken)
    {
        return await _context.Fields
            .AnyAsync(f => f.Id == fieldId && f.IsActive, cancellationToken);
    }

    public async Task<bool> SportExists(int sportId, CancellationToken cancellationToken)
    {
        return await _context.Sports
            .AnyAsync(s => s.Id == sportId, cancellationToken);
    }

    public async Task<bool> BeAvailableTimeSlot(CreateBookingCommand model, CancellationToken cancellationToken)
    {
        var conflictingBookings = await _context.Bookings
            .Where(b => b.FieldId == model.FieldId 
                       && b.BookingDate.Date == model.BookingDate.Date
                       && b.Status != "Cancelled"
                       && ((model.StartTime >= b.StartTime && model.StartTime < b.EndTime) ||
                           (model.EndTime > b.StartTime && model.EndTime <= b.EndTime) ||
                           (model.StartTime <= b.StartTime && model.EndTime >= b.EndTime)))
            .AnyAsync(cancellationToken);

        return !conflictingBookings;
    }
}
