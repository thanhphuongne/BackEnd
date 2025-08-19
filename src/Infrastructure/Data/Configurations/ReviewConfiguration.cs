using BackEnd.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEnd.Infrastructure.Data.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Reviews");
        
        builder.Property(r => r.OverallRating)
            .IsRequired();

        builder.Property(r => r.FieldConditionRating)
            .IsRequired();

        builder.Property(r => r.FacilitiesRating)
            .IsRequired();

        builder.Property(r => r.StaffRating)
            .IsRequired();

        builder.Property(r => r.ValueRating)
            .IsRequired();

        builder.Property(r => r.ReviewText)
            .HasColumnType("text");

        builder.Property(r => r.Photos)
            .HasColumnType("nvarchar(max)");

        // Indexes
        builder.HasIndex(r => r.BookingId)
            .IsUnique();
        builder.HasIndex(r => r.FieldId);

        // Foreign key relationships
        builder.HasOne(r => r.User)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Field)
            .WithMany(f => f.Reviews)
            .HasForeignKey(r => r.FieldId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
