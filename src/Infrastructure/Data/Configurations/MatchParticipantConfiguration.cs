using BackEnd.Domain.Entities;
using BackEnd.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEnd.Infrastructure.Data.Configurations;

public class MatchParticipantConfiguration : IEntityTypeConfiguration<MatchParticipant>
{
    public void Configure(EntityTypeBuilder<MatchParticipant> builder)
    {
        builder.ToTable("MatchParticipants");
        
        builder.Property(mp => mp.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(mp => mp.JoinedAt)
            .IsRequired();

        // Indexes
        builder.HasIndex(mp => new { mp.MatchId, mp.UserId })
            .IsUnique();

        // Foreign key relationships
        builder.HasOne(mp => mp.Match)
            .WithMany(m => m.Participants)
            .HasForeignKey(mp => mp.MatchId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(mp => mp.User)
            .WithMany(u => u.MatchParticipations)
            .HasForeignKey(mp => mp.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
