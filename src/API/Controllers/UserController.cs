using API.Handlers;
using API.Helpers;
using Application.DTOs.User;
using Application.Interfaces;
using Domain.Common.ResultPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/user")]
[Authorize(Roles = "Admin")]
public class UserController(
    IUserServices userServices,
    IAccessTokenHelper accessTokenHelper,
    ILogger<UserController> logger) : Controller
{
    private string AccessToken => accessTokenHelper.GetAccessToken();

    // GET: api/user/GetUserProfiles/id
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetProfileById()
    {
        logger.LogInformation("Retrieving user profile");
        var result = await userServices.GetProfilesAsync();
        return result.Match(
            success =>
            {
                logger.LogInformation("Successfully retrieved user profile");
                return Ok(result.Value);
            },
            error =>
            {
                logger.LogError("Failed to retrieve user profile: {Error}", error.Description);
                return error.ToActionResult();
            });
    }

    // PUT: api/user/UpdateUserProfile
    [HttpPatch]
    public async Task<IActionResult> UpdateUserProfile([FromBody] UserRequestDto request)
    {
        logger.LogInformation("Updating user profile");
        var result = await userServices.UpdateProfileAsync(request, AccessToken);
        return result.Match(
            success =>
            {
                logger.LogInformation("Successfully updated user profile");
                return Ok(new { message = "Profile updated successfully" });
            },
            error =>
            {
                logger.LogError("Failed to update user profile: {Error}", error.Description);
                return error.ToActionResult();
            });
    }
}