using BackEnd.Domain.Entities;

namespace BackEnd.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Sport> Sports { get; }

    DbSet<Field> Fields { get; }

    DbSet<User> CustomUsers { get; }

    DbSet<Pricing> Pricings { get; }

    DbSet<Booking> Bookings { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
