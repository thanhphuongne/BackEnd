using BackEnd.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEnd.Infrastructure.Data.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.Property(b => b.TotalPrice)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(b => b.Status)
            .HasMaxLength(50)
            .HasDefaultValue("Pending")
            .IsRequired();

        builder.Property(b => b.BookingDate)
            .HasColumnType("date")
            .IsRequired();

        // Foreign key relationships
        builder.HasOne(b => b.Customer)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(b => b.Field)
            .WithMany(f => f.Bookings)
            .HasForeignKey(b => b.FieldId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Sport)
            .WithMany(s => s.Bookings)
            .HasForeignKey(b => b.SportId)
            .OnDelete(DeleteBehavior.Restrict);

        // Index for efficient querying
        builder.HasIndex(b => new { b.FieldId, b.BookingDate, b.StartTime, b.EndTime });
        builder.HasIndex(b => b.CustomerId);
        builder.HasIndex(b => b.BookingDate);
    }
}
