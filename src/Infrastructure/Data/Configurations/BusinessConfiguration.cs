using BackEnd.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEnd.Infrastructure.Data.Configurations;

public class BusinessConfiguration : IEntityTypeConfiguration<Business>
{
    public void Configure(EntityTypeBuilder<Business> builder)
    {
        builder.ToTable("Businesses");
        
        builder.Property(b => b.BusinessName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(b => b.ContactEmail)
            .HasMaxLength(255);

        builder.Property(b => b.ContactPhone)
            .HasMaxLength(20);

        builder.Property(b => b.Address)
            .HasColumnType("text")
            .IsRequired();

        // Indexes
        builder.HasIndex(b => b.OwnerId)
            .IsUnique();
    }
}
