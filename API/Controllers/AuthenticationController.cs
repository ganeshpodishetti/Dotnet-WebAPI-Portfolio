using API.Helpers;
using Application.DTOs.Authentication;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/authentication")]
public class AuthenticationController(
    IAuthenticationService authenticationService,
    IAccessTokenHelper accessTokenHelper) : Controller
{
    private string AccessToken => accessTokenHelper.GetAccessToken();

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            return BadRequest("Email or password are required");

        if (string.IsNullOrEmpty(request.UserName))
            return BadRequest("User name required");

        var response = await authenticationService.RegisterAsync(request);
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        // Pre-validate request to fail fast
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            return BadRequest("Email and password are required");
        var response = await authenticationService.LoginAsync(request);
        return Ok(response);
    }

    [HttpPost("changePassword")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto request)
    {
        var user = await authenticationService.ChangePasswordAsync(request, AccessToken);
        if (user)
            return Ok("Password changed successfully");
        return BadRequest("Failed to change password. Please try again.");
    }

    [HttpDelete("deleteUser")]
    [Authorize]
    public async Task<IActionResult> DeleteUser()
    {
        var user = await authenticationService.DeleteUserAsync(AccessToken);
        if (user)
            return Ok("User deleted successfully");
        return BadRequest("Failed to delete user. Please try again.");
    }
}