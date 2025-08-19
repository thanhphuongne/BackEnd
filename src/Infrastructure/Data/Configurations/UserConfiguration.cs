using BackEnd.Domain.Entities;
using BackEnd.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEnd.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("AppUsers");
        
        builder.Property(u => u.Email)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(u => u.Phone)
            .HasMaxLength(20);

        builder.Property(u => u.FullName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.DisplayName)
            .HasMaxLength(100);

        builder.Property(u => u.PasswordHash)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(u => u.ProfilePhotoUrl)
            .HasMaxLength(255);

        builder.Property(u => u.AccountType)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(u => u.VerificationCode)
            .HasMaxLength(10);

        // Indexes
        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.HasIndex(u => u.Phone)
            .IsUnique();

        // Relationships
        builder.HasOne(u => u.Business)
            .WithOne(b => b.Owner)
            .HasForeignKey<Business>(b => b.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(u => u.UserPreference)
            .WithOne(up => up.User)
            .HasForeignKey<UserPreference>(up => up.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
