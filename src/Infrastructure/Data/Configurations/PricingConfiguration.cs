using BackEnd.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEnd.Infrastructure.Data.Configurations;

public class PricingConfiguration : IEntityTypeConfiguration<Pricing>
{
    public void Configure(EntityTypeBuilder<Pricing> builder)
    {
        builder.Property(p => p.PricePerHour)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(p => p.DayOfWeek)
            .HasConversion<int>();

        // Foreign key relationships
        builder.HasOne(p => p.Sport)
            .WithMany(s => s.Pricings)
            .HasForeignKey(p => p.SportId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.Field)
            .WithMany(f => f.Pricings)
            .HasForeignKey(p => p.FieldId)
            .OnDelete(DeleteBehavior.Restrict);

        // Composite unique index to prevent duplicate pricing for same field/sport/day/time
        builder.HasIndex(p => new { p.SportId, p.FieldId, p.DayOfWeek, p.StartTime, p.EndTime })
            .IsUnique();
    }
}
