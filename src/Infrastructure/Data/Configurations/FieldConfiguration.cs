using BackEnd.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEnd.Infrastructure.Data.Configurations;

public class FieldConfiguration : IEntityTypeConfiguration<Field>
{
    public void Configure(EntityTypeBuilder<Field> builder)
    {
        builder.Property(f => f.FieldName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(f => f.Location)
            .HasMaxLength(200);

        builder.Property(f => f.City)
            .HasMaxLength(100);

        builder.Property(f => f.District)
            .HasMaxLength(100);

        builder.Property(f => f.Address)
            .HasMaxLength(300);

        builder.Property(f => f.Description)
            .HasMaxLength(500);

        builder.Property(f => f.FieldType)
            .HasMaxLength(50);

        builder.Property(f => f.ImageUrl)
            .HasMaxLength(500);

        builder.Property(f => f.Rating)
            .HasColumnType("decimal(3,2)");

        builder.Property(f => f.IsActive)
            .HasDefaultValue(true);

        // Foreign key relationships
        builder.HasOne(f => f.Sport)
            .WithMany(s => s.Fields)
            .HasForeignKey(f => f.SportId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(f => f.Owner)
            .WithMany(u => u.OwnedFields)
            .HasForeignKey(f => f.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(f => new { f.SportId, f.FieldName })
            .IsUnique();

        builder.HasIndex(f => new { f.City, f.District, f.IsActive });
        builder.HasIndex(f => new { f.OwnerId, f.IsActive });
    }
}
