using BackEnd.Domain.Entities;
using BackEnd.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEnd.Infrastructure.Data.Configurations;

public class MatchConfiguration : IEntityTypeConfiguration<Match>
{
    public void Configure(EntityTypeBuilder<Match> builder)
    {
        builder.ToTable("Matches");
        
        builder.Property(m => m.SportType)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(m => m.StartTime)
            .IsRequired();

        builder.Property(m => m.EndTime)
            .IsRequired();

        builder.Property(m => m.SkillLevel)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(m => m.MaxPlayers)
            .IsRequired();

        builder.Property(m => m.CurrentPlayers)
            .HasDefaultValue(0);

        builder.Property(m => m.Description)
            .HasColumnType("text");

        builder.Property(m => m.Privacy)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(m => m.Status)
            .HasConversion<string>()
            .IsRequired();

        // Indexes
        builder.HasIndex(m => m.FieldId);

        // Foreign key relationships
        builder.HasOne(m => m.Organizer)
            .WithMany(u => u.OrganizedMatches)
            .HasForeignKey(m => m.OrganizerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.Field)
            .WithMany(f => f.Matches)
            .HasForeignKey(m => m.FieldId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
