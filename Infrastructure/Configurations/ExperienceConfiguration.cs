using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class ExperienceConfiguration : IEntityTypeConfiguration<Experience>
{
    public void Configure(EntityTypeBuilder<Experience> builder)
    {
        builder.Property(t => t.CompanyName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(t => t.StartDate)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(t => t.EndDate)
            .HasMaxLength(10);

        builder.Property(t => t.Description)
            .HasMaxLength(1000);

        builder.Property(t => t.Location)
            .HasMaxLength(100);
        
        builder.Property(t => t.Title)
            .HasMaxLength(100)
            .IsRequired();
    }
}