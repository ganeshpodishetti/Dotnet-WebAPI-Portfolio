using System.Reflection;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

internal class PortfolioDbContext(DbContextOptions<PortfolioDbContext> options) 
    : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options)
{
    internal DbSet<Education> Educations { get; set; } = null!;
    internal DbSet<Experience> Experiences { get; set; } = null!;
    internal DbSet<Project> Projects { get; set; } = null!;
    internal DbSet<Skill> Skills { get; set; } = null!;
    internal DbSet<SocialLink> SocialLinks { get; set; } = null!;
    internal DbSet<Message> Messages { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
           
        // Remove AspNet prefix from tables
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();
            if (tableName!.StartsWith("AspNet"))
            {
                entityType.SetTableName(tableName[6..]);
            }
        }
    }
}