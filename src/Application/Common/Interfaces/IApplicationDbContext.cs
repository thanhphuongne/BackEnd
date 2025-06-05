using BackEnd.Domain.Entities;

namespace BackEnd.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Sport> Sports { get; }

    DbSet<Field> Fields { get; }

    DbSet<User> CustomUsers { get; }

    DbSet<Pricing> Pricings { get; }

    DbSet<Booking> Bookings { get; }

    DbSet<Equipment> Equipment { get; }

    DbSet<TeamMatch> TeamMatches { get; }

    DbSet<Payment> Payments { get; }

    DbSet<Notification> Notifications { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
