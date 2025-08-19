using BackEnd.Domain.Entities;
using BackEnd.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEnd.Infrastructure.Data.Configurations;

public class FieldConfiguration : IEntityTypeConfiguration<Field>
{
    public void Configure(EntityTypeBuilder<Field> builder)
    {
        builder.ToTable("Fields");

        builder.Property(f => f.FieldName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(f => f.SportType)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(f => f.Address)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(f => f.Description)
            .HasColumnType("text");

        builder.Property(f => f.Photos)
            .HasColumnType("nvarchar(max)");

        builder.Property(f => f.OperatingHours)
            .HasColumnType("nvarchar(max)");

        builder.Property(f => f.BasePrice)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(f => f.IsActive)
            .HasDefaultValue(true);

        // Indexes
        builder.HasIndex(f => f.BusinessId);

        // Foreign key relationship
        builder.HasOne(f => f.Business)
            .WithMany(b => b.Fields)
            .HasForeignKey(f => f.BusinessId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
