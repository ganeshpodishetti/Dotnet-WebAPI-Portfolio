using Domain.Common;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using UserErrors = Domain.Errors.UserErrors;

namespace Infrastructure.Repositories;

internal class AuthenticationRepository(UserManager<User> userManager)
    : IAuthenticationRepository
{
    // login a user
    public async Task<Result<bool>> UpdateUserAsync(User user)
    {
        var result = await userManager.UpdateAsync(user);
        if (result.Succeeded) return Result.Success(true);

        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        return Result<bool>.Failure(UserErrors.FailedToUpdateUser(errors));
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

    public async Task<Result<bool>> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
            return Result<bool>.Failure(UserErrors.UserNotFound(userId));

        var result = await userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        if (result.Succeeded) return Result.Success(true);

        var enumerator = result.Errors.First().Description;
        return Result.Failure<bool>(UserErrors.FailedToChangePassword(enumerator));
    }

    // register a new user
    public async Task<Result<User>> RegisterUserAsync(User user, string password)
    {
        // Check if username exists
        if (await userManager.FindByNameAsync(user.UserName!) != null)
            return Result.Failure<User>(UserErrors.UserAlreadyExists(user.UserName!));

        user.UserName = user.UserName!.ToLower();
        user.PasswordHash = userManager.PasswordHasher.HashPassword(user, password);

        var result = await userManager.CreateAsync(user, password);
        if (result.Succeeded) return Result.Success(user);

        var enumerator = result.Errors.First().Description;
        return Result.Failure<User>(UserErrors.FailedToRegisterUser(enumerator));
    }

    // Delete a user
    public async Task<Result<bool>> DeleteUserAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
            return Result<bool>.Failure(UserErrors.UserNotFound(userId));

        var result = await userManager.DeleteAsync(user);
        if (result.Succeeded) return Result.Success(true);

        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        return Result<bool>.Failure(UserErrors.FailedToDeleteUser(errors));
    }
}