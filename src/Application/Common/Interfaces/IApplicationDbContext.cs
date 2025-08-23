using BackEnd.Domain.Entities;

namespace BackEnd.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Sport> Sports { get; }
    DbSet<Business> Businesses { get; }
    DbSet<Venue> Venues { get; }
    DbSet<Field> Fields { get; }
    DbSet<FieldAvailability> FieldAvailabilities { get; }
    DbSet<PricingRule> PricingRules { get; }
    DbSet<Booking> Bookings { get; }
    DbSet<PaymentMethod> PaymentMethods { get; }
    DbSet<Payment> Payments { get; }
    DbSet<Match> Matches { get; }
    DbSet<MatchParticipant> MatchParticipants { get; }
    DbSet<MatchChat> MatchChats { get; }
    DbSet<Review> Reviews { get; }
    DbSet<Notification> Notifications { get; }
    DbSet<UserPreference> UserPreferences { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
