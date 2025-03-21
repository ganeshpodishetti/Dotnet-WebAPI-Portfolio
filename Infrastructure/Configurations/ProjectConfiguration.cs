using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.Property(t => t.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(t => t.Description)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(t => t.Url)
            .HasMaxLength(50);

        builder.Property(t => t.GithubUrl)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(t => t.Skills)
            .HasMaxLength(2500);
    }
}