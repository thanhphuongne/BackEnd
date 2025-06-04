using BackEnd.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEnd.Infrastructure.Data.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasKey(b => b.BookingID);
        
        builder.Property(b => b.TotalPrice)
            .HasPrecision(10, 2);
            
        builder.Property(b => b.Status)
            .HasMaxLength(50)
            .IsRequired();
            
        // Change cascade delete behavior to restrict
        builder.HasOne(b => b.Customer)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.CustomerID)
            .OnDelete(DeleteBehavior.Restrict);
            
        builder.HasOne(b => b.Field)
            .WithMany(f => f.Bookings)
            .HasForeignKey(b => b.FieldID)
            .OnDelete(DeleteBehavior.Restrict);
            
        builder.HasOne(b => b.Sport)
            .WithMany(s => s.Bookings)
            .HasForeignKey(b => b.SportID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
