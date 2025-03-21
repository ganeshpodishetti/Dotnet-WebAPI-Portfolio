using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class EducationConfiguration : IEntityTypeConfiguration<Education>
{
    public void Configure(EntityTypeBuilder<Education> builder)
    {
        builder.Property(t => t.Degree)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(t => t.FieldOfStudy)
            .HasMaxLength(250);

        builder.Property(t => t.StartDate)
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(t => t.Location)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(t => t.EndDate)
            .HasMaxLength(10);

        builder.Property(t => t.School)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(t => t.Description)
            .HasMaxLength(2500);
    }
}