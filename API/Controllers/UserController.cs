using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/user")]
public class UserController(IUserServices userServices) : Controller
{
    [HttpGet("GetUserProfiles/{userId}")]
    public async Task<IActionResult> GetUserProfiles(string userId)
    {
        var result = await userServices.GetProfileByIdAsync(userId);
        return Ok(result);
    }

    [HttpPost("AddUserProfile")]
    public async Task<IActionResult> AddUserProfile(UserProfileDto userProfileDto)
    {
        var result = await userServices.AddProfileAsync(userProfileDto);
        return Ok(result);
    }
}