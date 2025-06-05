using BackEnd.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEnd.Infrastructure.Data.Configurations;

public class TeamMatchConfiguration : IEntityTypeConfiguration<TeamMatch>
{
    public void Configure(EntityTypeBuilder<TeamMatch> builder)
    {
        builder.Property(tm => tm.MatchDate)
            .HasColumnType("date")
            .IsRequired();

        builder.Property(tm => tm.StartTime)
            .HasColumnType("time")
            .IsRequired();

        builder.Property(tm => tm.EndTime)
            .HasColumnType("time")
            .IsRequired();

        builder.Property(tm => tm.SkillLevel)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(tm => tm.Status)
            .HasMaxLength(50)
            .IsRequired()
            .HasDefaultValue("Looking");

        builder.Property(tm => tm.Description)
            .HasMaxLength(1000);

        builder.Property(tm => tm.ContactInfo)
            .HasMaxLength(500);

        // Foreign key relationships
        builder.HasOne(tm => tm.Creator)
            .WithMany(u => u.TeamMatches)
            .HasForeignKey(tm => tm.CreatorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(tm => tm.Opponent)
            .WithMany()
            .HasForeignKey(tm => tm.OpponentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(tm => tm.Sport)
            .WithMany()
            .HasForeignKey(tm => tm.SportId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(tm => tm.Field)
            .WithMany()
            .HasForeignKey(tm => tm.FieldId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(tm => new { tm.SportId, tm.MatchDate, tm.Status });
        builder.HasIndex(tm => new { tm.CreatorId, tm.MatchDate });
    }
}
