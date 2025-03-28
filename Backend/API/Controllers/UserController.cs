using API.Handlers;
using API.Helpers;
using Application.DTOs.User;
using Application.Interfaces;
using Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/user")]
[Authorize(Roles = "Admin")]
public class UserController(
    IUserServices userServices,
    IAccessTokenHelper accessTokenHelper) : Controller
{
    private string AccessToken => accessTokenHelper.GetAccessToken();

    // GET: api/user/GetUserProfiles/id
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetProfileById()
    {
        var result = await userServices.GetProfileByIdAsync(AccessToken);
        return result.Match(
            success => Ok(result.Value),
            error => error.ToActionResult());
    }

    // PUT: api/user/UpdateUserProfile
    [HttpPatch]
    public async Task<IActionResult> UpdateUserProfile([FromBody] UserRequestDto request)
    {
        var result = await userServices.UpdateProfileAsync(request, AccessToken);
        return result.Match(
            success => Ok(new { message = "Profile updated successfully" }),
            error => error.ToActionResult());
    }
}