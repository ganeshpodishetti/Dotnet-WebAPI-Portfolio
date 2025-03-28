using API.Handlers;
using API.Helpers;
using Application.DTOs.Authentication;
using Application.Interfaces;
using Domain.Common;
using Domain.Interfaces;
using Domain.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers;

[ApiController]
[Route("api/authentication")]
public class AuthenticationController(
    IAuthenticationService authenticationService,
    IAccessTokenHelper accessTokenHelper,
    IJwtTokenService jwtTokenService,
    IOptions<JwtTokenOptions> jwtOptions) : Controller
{
    private string AccessToken => accessTokenHelper.GetAccessToken();

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        var response = await authenticationService.RegisterAsync(request);
        return response.Match(
            success => Ok(response.Value),
            error => error.ToActionResult()
        );
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var response = await authenticationService.LoginAsync(request);
        return response.Match(
            success => Ok(response.Value),
            error => error.ToActionResult());
    }

    [HttpPost("changePassword")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto request)
    {
        var user = await authenticationService.ChangePasswordAsync(request, AccessToken);
        return user.Match(
            success => Ok(new { message = "Password changed successfully" }),
            error => error.ToActionResult());
    }

    [HttpDelete("deleteUser")]
    [Authorize]
    public async Task<IActionResult> DeleteUser()
    {
        var user = await authenticationService.DeleteUserAsync(AccessToken);
        return user.Match(
            success => Ok(new { message = "User deleted successfully" }),
            error => error.ToActionResult());
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
    {
        try
        {
            var (newAccessToken, newRefreshToken) = await jwtTokenService.RefreshTokenAsync(
                request.AccessToken,
                request.RefreshToken);

            return Ok(new RefreshTokenResponseDto
            {
                AccessToken = newAccessToken,
                AccessTokenExpirationAtUtc = DateTime.UtcNow.AddMinutes(jwtOptions.Value.AccessTokenExpirationMinutes)
                    .ToString("yyyy-MM-dd HH:mm:ss tt"),
                RefreshToken = newRefreshToken,
                RefreshTokenExpirationAtUtc = DateTime.UtcNow.AddDays(jwtOptions.Value.RefreshTokenExpirationDays)
                    .ToString("yyyy-MM-dd HH:mm:ss tt")
            });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    [HttpGet("is-authenticated")]
    public IActionResult IsAuthenticated()
    {
        var token = AccessToken;
        var isValid = jwtTokenService.ValidateCurrentToken(token);
        return Ok(new { isAuthenticated = isValid });
    }
}