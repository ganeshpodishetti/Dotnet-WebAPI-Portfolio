using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasMany(u => u.Educations)
            .WithOne()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Projects)
            .WithOne()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(u => u.Experiences)
            .WithOne()
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.SocialLinks)
            .WithOne()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.Skills)
            .WithOne()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.ToTable("Users").OwnsOne(u => u.Profile, profileBuilder =>
        {
            profileBuilder.Property(p => p.FirstName).HasMaxLength(50).IsRequired();
            profileBuilder.Property(p => p.LastName).HasMaxLength(50).IsRequired();
            profileBuilder.Property(p => p.ProfilePicture).HasMaxLength(2000);
            profileBuilder.Property(p => p.Bio).HasMaxLength(2500);
            profileBuilder.Property(p => p.Headline).HasMaxLength(250);
            profileBuilder.Property(p => p.Country).HasMaxLength(50);
            profileBuilder.Property(p => p.City).HasMaxLength(50);
        });
    }
}