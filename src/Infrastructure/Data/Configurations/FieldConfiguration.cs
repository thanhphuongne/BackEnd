using BackEnd.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEnd.Infrastructure.Data.Configurations;

public class FieldConfiguration : IEntityTypeConfiguration<Field>
{
    public void Configure(EntityTypeBuilder<Field> builder)
    {
        builder.HasKey(f => f.FieldID);
        
        builder.Property(f => f.FieldName)
            .HasMaxLength(100)
            .IsRequired();
            
        builder.Property(f => f.Location)
            .HasMaxLength(200)
            .IsRequired();
            
        builder.Property(f => f.Description)
            .HasMaxLength(500);
            
        // Keep cascade delete for this relationship
        builder.HasOne(f => f.Sport)
            .WithMany(s => s.Fields)
            .HasForeignKey(f => f.SportID);
    }
}
