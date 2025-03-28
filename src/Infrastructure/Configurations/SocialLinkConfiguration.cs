using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class SocialLinkConfiguration : IEntityTypeConfiguration<SocialLink>
{
    public void Configure(EntityTypeBuilder<SocialLink> builder)
    {
        builder.Property(t => t.Platform)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(t => t.Url)
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(t => t.Icon)
            .HasMaxLength(100);

        builder.HasOne<User>()
            .WithMany(u => u.SocialLinks)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}