using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class SkillConfiguration : IEntityTypeConfiguration<Skill>
{
    public void Configure(EntityTypeBuilder<Skill> builder)
    {
        builder.Property(t => t.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(t => t.Proficiency)
            .HasMaxLength(10);

        builder.Property(t => t.YearsOfExperience)
            .IsRequired();
    }
}