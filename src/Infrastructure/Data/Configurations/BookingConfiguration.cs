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

        builder.Property(b => b.Notes)
            .HasMaxLength(1000);

        builder.Property(b => b.EquipmentRented)
            .HasMaxLength(2000);

        builder.Property(b => b.EquipmentCost)
            .HasColumnType("decimal(18,2)");

        builder.Property(b => b.CancellationReason)
            .HasMaxLength(500);

        builder.Property(b => b.RecurrencePattern)
            .HasMaxLength(100);

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

        builder.HasOne(b => b.TeamMatch)
            .WithMany(tm => tm.Bookings)
            .HasForeignKey(b => b.TeamMatchId)
            .OnDelete(DeleteBehavior.Restrict);

        // Index for efficient querying
        builder.HasIndex(b => new { b.FieldId, b.BookingDate, b.StartTime, b.EndTime });
        builder.HasIndex(b => b.CustomerId);
        builder.HasIndex(b => b.BookingDate);
        builder.HasIndex(b => new { b.Status, b.BookingDate });
    }
}
