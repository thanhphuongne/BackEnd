using BackEnd.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEnd.Infrastructure.Data.Configurations;

public class PricingConfiguration : IEntityTypeConfiguration<Pricing>
{
    public void Configure(EntityTypeBuilder<Pricing> builder)
    {
        builder.HasKey(p => p.PricingID);
        
        builder.Property(p => p.DayOfWeek)
            .HasMaxLength(20)
            .IsRequired();
            
        builder.Property(p => p.PricePerHour)
            .HasPrecision(10, 2);
            
        // Change cascade delete behavior to restrict
        builder.HasOne(p => p.Sport)
            .WithMany(s => s.Pricings)
            .HasForeignKey(p => p.SportID)
            .OnDelete(DeleteBehavior.Restrict);
            
        builder.HasOne(p => p.Field)
            .WithMany(f => f.Pricings)
            .HasForeignKey(p => p.FieldID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
