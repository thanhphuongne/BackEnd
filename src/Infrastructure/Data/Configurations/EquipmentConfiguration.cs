using BackEnd.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEnd.Infrastructure.Data.Configurations;

public class EquipmentConfiguration : IEntityTypeConfiguration<Equipment>
{
    public void Configure(EntityTypeBuilder<Equipment> builder)
    {
        builder.Property(e => e.EquipmentName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasMaxLength(500);

        builder.Property(e => e.RentalPricePerHour)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(e => e.Condition)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.ImageUrl)
            .HasMaxLength(500);

        // Foreign key relationships
        builder.HasOne(e => e.Field)
            .WithMany(f => f.Equipment)
            .HasForeignKey(e => e.FieldId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(e => new { e.FieldId, e.EquipmentName })
            .IsUnique();
    }
}
