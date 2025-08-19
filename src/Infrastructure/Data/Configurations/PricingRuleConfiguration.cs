using BackEnd.Domain.Entities;
using BackEnd.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BackEnd.Infrastructure.Data.Configurations;

public class PricingRuleConfiguration : IEntityTypeConfiguration<PricingRule>
{
    public void Configure(EntityTypeBuilder<PricingRule> builder)
    {
        builder.ToTable("PricingRules");
        
        builder.Property(pr => pr.DayOfWeek)
            .HasConversion<string>();

        builder.Property(pr => pr.PriceModifier)
            .HasColumnType("decimal(5,2)");

        builder.Property(pr => pr.IsHoliday)
            .HasDefaultValue(false);

        // Indexes
        builder.HasIndex(pr => pr.FieldId);

        // Foreign key relationship
        builder.HasOne(pr => pr.Field)
            .WithMany(f => f.PricingRules)
            .HasForeignKey(pr => pr.FieldId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
