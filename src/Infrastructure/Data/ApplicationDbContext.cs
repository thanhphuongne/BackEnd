using System.Reflection;
using BackEnd.Application.Common.Interfaces;
using BackEnd.Domain.Entities;
using BackEnd.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Sport> Sports => Set<Sport>();

    public DbSet<Field> Fields => Set<Field>();

    public DbSet<User> CustomUsers => Set<User>();

    public DbSet<Pricing> Pricings => Set<Pricing>();

    public DbSet<Booking> Bookings => Set<Booking>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
