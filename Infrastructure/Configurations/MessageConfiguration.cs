using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.Property(t => t.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(t => t.Email)
            .HasMaxLength(320)
            .IsRequired();

        builder.Property(t => t.Content)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(t => t.IsRead)
            .IsRequired();

        builder.Property(t => t.Subject)
            .HasMaxLength(250);

        builder.HasOne<User>()
            .WithMany(u => u.Messages)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}