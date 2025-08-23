using BackEnd.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEnd.Infrastructure.Data.Configurations;

public class VenueConfiguration : IEntityTypeConfiguration<Venue>
{
    public void Configure(EntityTypeBuilder<Venue> builder)
    {
        builder.Property(v => v.VenueName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(v => v.Address)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(v => v.Description)
            .HasMaxLength(1000);

        builder.Property(v => v.ContactPhone)
            .HasMaxLength(20);

        builder.Property(v => v.ContactEmail)
            .HasMaxLength(100);

        builder.Property(v => v.OperatingHours)
            .HasMaxLength(100);

        builder.Property(v => v.Facilities)
            .HasMaxLength(500);

        // Relationships
        builder.HasOne(v => v.Business)
            .WithMany()
            .HasForeignKey(v => v.BusinessId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

        builder.HasMany(v => v.Fields)
            .WithOne(f => f.Venue)
            .HasForeignKey(f => f.VenueId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete
    }
}
