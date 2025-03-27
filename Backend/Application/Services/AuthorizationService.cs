using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public class AuthorizationService(
    RoleManager<IdentityRole<Guid>> roleManager,
    UserManager<User> userManager) : IUserAuthorizationService
{
    public async Task<Result<bool>> AssignRoleAsync(string userId, string roleName)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
            return Result<bool>.Failure(new GeneralError("User not found",
                $"User with ID {userId} not found",
                StatusCode.NotFound));

        var roleExists = await roleManager.RoleExistsAsync(roleName);
        if (!roleExists)
            return Result<bool>.Failure(new GeneralError("Role not found",
                $"Role {roleName} does not exist",
                StatusCode.NotFound));

        var result = await userManager.AddToRoleAsync(user, roleName);
        return result.Succeeded
            ? Result<bool>.Success(result.Succeeded)
            : Result<bool>.Failure(new GeneralError("Role assignment failed",
                $"User {user.UserName} assignment to role {roleName} failed",
                StatusCode.Failure));
    }

    public async Task<Result<bool>> RemoveRoleAsync(string userId, string roleName)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
            return Result<bool>.Failure(new GeneralError("User not found",
                $"User with ID {userId} not found",
                StatusCode.NotFound));

        var isInRole = await userManager.IsInRoleAsync(user, roleName);
        if (!isInRole)
            return Result<bool>.Failure(new GeneralError("User not in role",
                $"User {user.UserName} is not in role {roleName}",
                StatusCode.Failure));

        var result = await userManager.RemoveFromRoleAsync(user, roleName);
        return result.Succeeded
            ? Result<bool>.Success(result.Succeeded)
            : Result<bool>.Failure(new GeneralError("Role removal failed",
                $"User {user.UserName} removal from role {roleName} failed",
                StatusCode.Failure));
    }

    public async Task<Result<bool>> CreateRoleAsync(string roleName)
    {
        var roleExists = await roleManager.RoleExistsAsync(roleName);
        if (roleExists)
            return Result<bool>.Failure(new GeneralError("Role already exists",
                $"Role {roleName} already exists",
                StatusCode.Conflict));

        var result = await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
        return result.Succeeded
            ? Result<bool>.Success(result.Succeeded)
            : Result<bool>.Failure(new GeneralError("Role creation failed",
                $"Role {roleName} creation failed",
                StatusCode.Failure));
    }

    public async Task<Result<IEnumerable<string>>> GetUserRolesAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
            return Result<IEnumerable<string>>.Failure(new GeneralError("User not found",
                $"User with ID {userId} not found",
                StatusCode.NotFound));

        var roles = await userManager.GetRolesAsync(user);
        return Result<IEnumerable<string>>.Success(roles);
    }
}