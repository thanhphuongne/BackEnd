using BackEnd.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEnd.Infrastructure.Data.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.Property(p => p.Amount)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(p => p.PaymentMethod)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.Status)
            .HasMaxLength(50)
            .IsRequired()
            .HasDefaultValue("Pending");

        builder.Property(p => p.TransactionId)
            .HasMaxLength(100);

        builder.Property(p => p.PaymentGatewayResponse)
            .HasMaxLength(2000);

        builder.Property(p => p.RefundAmount)
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.RefundReason)
            .HasMaxLength(500);

        builder.Property(p => p.InvoiceNumber)
            .HasMaxLength(50);

        builder.Property(p => p.Notes)
            .HasMaxLength(1000);

        // Foreign key relationships
        builder.HasOne(p => p.Booking)
            .WithMany(b => b.Payments)
            .HasForeignKey(p => p.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.Customer)
            .WithMany(u => u.Payments)
            .HasForeignKey(p => p.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(p => p.TransactionId)
            .IsUnique()
            .HasFilter("[TransactionId] IS NOT NULL");

        builder.HasIndex(p => p.InvoiceNumber)
            .IsUnique()
            .HasFilter("[InvoiceNumber] IS NOT NULL");

        builder.HasIndex(p => new { p.BookingId, p.Status });
        builder.HasIndex(p => new { p.CustomerId, p.Status });
    }
}
