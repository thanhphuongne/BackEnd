using BackEnd.Domain.Entities;
using BackEnd.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEnd.Infrastructure.Data.Configurations;

public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
{
    public void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        builder.ToTable("PaymentMethods");
        
        builder.Property(pm => pm.MethodType)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(pm => pm.LastFourDigits)
            .HasMaxLength(4);

        builder.Property(pm => pm.IsDefault)
            .HasDefaultValue(false);

        builder.Property(pm => pm.BillingAddress)
            .HasColumnType("text");

        // Indexes
        builder.HasIndex(pm => pm.UserId);

        // Foreign key relationship
        builder.HasOne(pm => pm.User)
            .WithMany(u => u.PaymentMethods)
            .HasForeignKey(pm => pm.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
