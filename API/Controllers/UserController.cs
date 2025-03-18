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
    [HttpGet]
    public async Task<IActionResult> GetProfileById()
    {
        var result = await userServices.GetProfileByIdAsync(AccessToken);
        return Ok(result);
    }

    // POST: api/user/AddUserProfile
    [HttpPost]
    public async Task<IActionResult> AddUserProfile([FromBody] UserRequestDto userRequestDto)
    {
        var result = await userServices.AddProfileAsync(userRequestDto, AccessToken);
        return Ok(result);
    }

    // PUT: api/user/UpdateUserProfile
    [HttpPatch]
    public async Task<IActionResult> UpdateUserProfile([FromBody] UserRequestDto userRequestDto)
    {
        var result = await userServices.UpdateProfileAsync(userRequestDto, AccessToken);
        return Ok(result);
    }
}