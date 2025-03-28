using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using UserErrors = Domain.Errors.UserErrors;

namespace Infrastructure.Repositories;

internal class AuthenticationRepository(
    UserManager<User> userManager,
    RoleManager<IdentityRole<Guid>> roleManager,
    SignInManager<User> signInManager)
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

    // login a user
    public async Task<Result<bool>> SignInUserAsync(User user, string password)
    {
        var result = await signInManager.PasswordSignInAsync(user, password, false, false);
        return result.Succeeded ? Result.Success(true) : Result<bool>.Failure(UserErrors.LoginFailed(user.Email!));
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

    public async Task<Result<bool>> AssignRoleAsync(User user, string roleName)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            var role = new IdentityRole<Guid>(roleName);
            var createRoleResult = await roleManager.CreateAsync(role);
            if (!createRoleResult.Succeeded)
                return Result<bool>.Failure(UserErrors.FailedToCreateRole(roleName));
        }

        var result = await userManager.AddToRoleAsync(user, roleName);
        if (result.Succeeded) return Result.Success(true);

        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        return Result<bool>.Failure(UserErrors.FailedToAssignRole(errors));
    }

    public async Task<bool> IsInRoleAsync(User user, string roleName)
    {
        return await userManager.IsInRoleAsync(user, roleName);
    }

    public async Task<Result<bool>> CheckAdminExistsAsync()
    {
        var adminRoleName = UserRole.Admin.ToString();

        // Check if Admin role exists
        if (!await roleManager.RoleExistsAsync(adminRoleName))
            return Result<bool>.Failure(new GeneralError("Admin exists", "Admin already exist.",
                StatusCode.UnAuthorized));

        // Check if any user has Admin role
        var usersInRole = await userManager.GetUsersInRoleAsync(adminRoleName);
        return Result.Success(usersInRole.Any());
    }
}