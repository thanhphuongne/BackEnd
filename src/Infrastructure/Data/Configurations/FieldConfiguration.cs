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

        builder.Property(f => f.Description)
            .HasMaxLength(500);

        builder.Property(f => f.IsActive)
            .HasDefaultValue(true);

        // Foreign key relationship
        builder.HasOne(f => f.Sport)
            .WithMany(s => s.Fields)
            .HasForeignKey(f => f.SportId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(f => new { f.SportId, f.FieldName })
            .IsUnique();
    }
}
