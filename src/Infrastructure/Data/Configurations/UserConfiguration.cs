using BackEnd.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEnd.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.UserID);
        
        builder.Property(u => u.UserName)
            .HasMaxLength(100)
            .IsRequired();
            
        builder.Property(u => u.Password)
            .HasMaxLength(100)
            .IsRequired();
            
        builder.Property(u => u.Phone)
            .HasMaxLength(20)
            .IsRequired();
            
        builder.Property(u => u.Email)
            .HasMaxLength(100)
            .IsRequired();
            
        builder.Property(u => u.Role)
            .HasMaxLength(50)
            .IsRequired();
            
        // Set table name to avoid conflict with ASP.NET Identity Users table
        builder.ToTable("AppUsers");
    }
}