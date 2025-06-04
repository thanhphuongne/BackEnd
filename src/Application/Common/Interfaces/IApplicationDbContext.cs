using BackEnd.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }
    DbSet<TodoItem> TodoItems { get; }
    
    // New entities
    DbSet<Sport> Sports { get; }
    DbSet<Field> Fields { get; }
    DbSet<User> AppUsers { get; } // Renamed from Users to AppUsers
    DbSet<Pricing> Pricings { get; }
    DbSet<Booking> Bookings { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
