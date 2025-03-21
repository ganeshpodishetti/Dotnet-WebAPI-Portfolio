using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class SkillConfiguration : IEntityTypeConfiguration<Skill>
{
    public void Configure(EntityTypeBuilder<Skill> builder)
    {
        builder.Property(t => t.SkillCategory)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(t => t.SkillsTypes)
            .HasMaxLength(2500)
            .IsRequired();
    }
}