using Application.DTOs.Identity;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/identity")]
public class IdentityController(IIdentityService identityService) : Controller
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            return BadRequest("Email or password are required");

        if (string.IsNullOrEmpty(request.UserName))
            return BadRequest("User name required");

        var response = await identityService.RegisterAsync(request);
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        // Pre-validate request to fail fast
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            return BadRequest("Email and password are required");
        var response = await identityService.LoginAsync(request);
        return Ok(response);
    }

    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto request,
        [FromHeader] string authorization)
    {
        var user = await identityService.ChangePasswordAsync(request, authorization);
        if (user)
            return Ok("Password changed successfully");
        return BadRequest("Failed to change password. Please try again.");
    }
}