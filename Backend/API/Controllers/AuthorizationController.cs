using API.Handlers;
using Application.Interfaces;
using Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/authorization")]
[Authorize]
public class AuthorizationController(
    IUserAuthorizationService authorizationService)
    : Controller
{
    // POST api/authorization/roles
    [HttpPost("roles")]
    public async Task<IActionResult> CreateRole(string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
            return BadRequest(new { error = "Role name cannot be empty" });

        var createRole = await authorizationService.CreateRoleAsync(roleName);
        return createRole.Match(
            success => Ok(new { message = $"Role {roleName} created successfully" }),
            error => error.ToActionResult());
    }

    // POST api/authorization/users/{userId}/roles/{roleName}
    [HttpPost("users/{userId}/roles/{roleName}")]
    public async Task<IActionResult> AssignRoleToUser(string userId, string roleName)
    {
        var assignRole = await authorizationService.AssignRoleAsync(userId, roleName);
        return assignRole.Match(
            success => Ok(new { message = $"Role {roleName} assigned to user with ID {userId} successfully" }),
            error => error.ToActionResult());
    }

    // DELETE api/authorization/users/{userId}/roles/{roleName}
    [HttpDelete("users/{userId}/roles/{roleName}")]
    public async Task<IActionResult> RemoveRoleFromUser(string userId, string roleName)
    {
        var removeRole = await authorizationService.RemoveRoleAsync(userId, roleName);
        return removeRole.Match(
            success => Ok(new { message = $"Role {roleName} removed from user with ID {userId} successfully" }),
            error => error.ToActionResult());
    }

    // GET api/authorization/users/{userId}/roles
    [HttpGet("users/{userId}/roles")]
    public async Task<IActionResult> GetUserRoles(string userId)
    {
        var roles = await authorizationService.GetUserRolesAsync(userId);
        return roles.Match(
            success => Ok(new { roles = success }),
            error => error.ToActionResult());
    }
}