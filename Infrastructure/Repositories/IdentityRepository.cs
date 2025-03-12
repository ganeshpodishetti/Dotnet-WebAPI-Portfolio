using System.Text.RegularExpressions;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Repositories;

internal partial class IdentityRepository(UserManager<User> userManager)
    : IIdentityRepository
{
    // register a new user
    public async Task<User> CreateUserAsync(User user, string password)
    {
        if (string.IsNullOrEmpty(user.UserName))
            throw new ArgumentException("Username is required");

        // Check for special characters and spaces
        if (user.UserName != SanitizeName(user.UserName))
            throw new ArgumentException("Username cannot contain special characters or spaces");

        // Check if username is unique
        if (await userManager.FindByNameAsync(user.UserName) != null)
            throw new Exception("Username is already taken");

        user.UserName = user.UserName.ToLower();
        user.PasswordHash = userManager.PasswordHasher.HashPassword(user, password);
        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            throw new Exception("Failed to create user");

        return user;
    }

    // login a user
    public async Task<User> UpdateUserAsync(User user)
    {
        var result = await userManager.UpdateAsync(user);
        if (result.Succeeded) return user;
        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        throw new Exception($"Failed to update user: {errors}");
    }

    // Validate user credentials
    public async Task<bool> ValidateCredentialsAsync(string email, string password)
    {
        var user = await userManager.FindByEmailAsync(email);
        return user != null && await userManager.CheckPasswordAsync(user, password);
    }

    // Find a user by email
    public async Task<User?> FindByEmailAsync(string email)
    {
        return await userManager.FindByEmailAsync(email);
    }

    // Find a user by username
    private static string SanitizeName(string name)
    {
        // Remove special characters and spaces
        return MyRegex().Replace(name, "");
    }

    // Regex for special characters and spaces
    [GeneratedRegex(@"[^a-zA-Z0-9.]")]
    private static partial Regex MyRegex();
}