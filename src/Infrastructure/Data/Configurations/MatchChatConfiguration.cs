using BackEnd.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEnd.Infrastructure.Data.Configurations;

public class MatchChatConfiguration : IEntityTypeConfiguration<MatchChat>
{
    public void Configure(EntityTypeBuilder<MatchChat> builder)
    {
        builder.ToTable("MatchChats");
        
        builder.Property(mc => mc.MessageText)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(mc => mc.SentAt)
            .IsRequired();

        // Indexes
        builder.HasIndex(mc => mc.MatchId);
        builder.HasIndex(mc => mc.UserId);

        // Foreign key relationships
        builder.HasOne(mc => mc.Match)
            .WithMany(m => m.Chats)
            .HasForeignKey(mc => mc.MatchId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(mc => mc.User)
            .WithMany(u => u.MatchChats)
            .HasForeignKey(mc => mc.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
