using BackEnd.Domain.Entities;
using BackEnd.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEnd.Infrastructure.Data.Configurations;

public class FieldAvailabilityConfiguration : IEntityTypeConfiguration<FieldAvailability>
{
    public void Configure(EntityTypeBuilder<FieldAvailability> builder)
    {
        builder.ToTable("FieldAvailability");
        
        builder.Property(fa => fa.StartTime)
            .IsRequired();

        builder.Property(fa => fa.EndTime)
            .IsRequired();

        builder.Property(fa => fa.Status)
            .HasConversion<string>()
            .IsRequired();

        // Indexes
        builder.HasIndex(fa => fa.FieldId);

        // Foreign key relationship
        builder.HasOne(fa => fa.Field)
            .WithMany(f => f.FieldAvailabilities)
            .HasForeignKey(fa => fa.FieldId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
