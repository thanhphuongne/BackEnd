using BackEnd.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEnd.Infrastructure.Data.Configurations;

public class UserPreferenceConfiguration : IEntityTypeConfiguration<UserPreference>
{
    public void Configure(EntityTypeBuilder<UserPreference> builder)
    {
        builder.ToTable("UserPreferences");
        
        builder.Property(up => up.FavoriteSports)
            .HasColumnType("nvarchar(max)");

        builder.Property(up => up.PreferredLocations)
            .HasColumnType("nvarchar(max)");

        builder.Property(up => up.NotificationSettings)
            .HasColumnType("nvarchar(max)");

        builder.Property(up => up.PrivacySettings)
            .HasColumnType("nvarchar(max)");

        // Indexes
        builder.HasIndex(up => up.UserId)
            .IsUnique();
    }
}
