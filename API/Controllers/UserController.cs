using API.Helpers;
using Application.DTOs.User;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/user")]
[Authorize]
public class UserController(
    IUserServices userServices,
    IAccessTokenHelper accessTokenHelper) : Controller
{
    private string AccessToken => accessTokenHelper.GetAccessToken();

    // GET: api/user/GetUserProfiles/id
    [HttpGet("GetProfileById")]
    public async Task<IActionResult> GetProfileById()
    {
        var result = await userServices.GetProfileByIdAsync(AccessToken);
        return Ok(result);
    }

    // POST: api/user/AddUserProfile
    [HttpPost("AddUserProfile")]
    public async Task<IActionResult> AddUserProfile([FromBody] UserProfileDto userProfileDto)
    {
        var result = await userServices.AddProfileAsync(userProfileDto, AccessToken);
        return Ok(result);
    }

    // PUT: api/user/UpdateUserProfile
    [HttpPatch("UpdateUserProfile")]
    public async Task<IActionResult> UpdateUserProfile([FromBody] UserProfileDto userProfileDto)
    {
        var result = await userServices.UpdateProfileAsync(userProfileDto, AccessToken);
        return Ok(result);
    }
}