using API.Handlers;
using API.Helpers;
using Application.DTOs.Authentication;
using Application.Interfaces;
using Domain.Common.ResultPattern;
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
    IOptions<JwtTokenOptions> jwtOptions,
    ILogger<AuthenticationController> logger) : Controller
{
    private string AccessToken => accessTokenHelper.GetAccessToken();

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        logger.LogInformation("Processing registration request for email: {Email}", request.Email);
        var response = await authenticationService.RegisterAsync(request);
        return response.Match(
            success =>
            {
                logger.LogInformation("Successfully registered user with email: {Email}", request.Email);
                return Ok(response.Value);
            },
            error =>
            {
                logger.LogError("Registration failed for email {Email}: {Error}", request.Email, error.Description);
                return error.ToActionResult();
            });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        logger.LogInformation("Processing login request for email: {Email}", request.Email);
        var response = await authenticationService.LoginAsync(request);
        return response.Match(
            success =>
            {
                logger.LogInformation("Successful login for email: {Email}", request.Email);
                return Ok(response.Value);
            },
            error =>
            {
                logger.LogWarning("Login failed for email {Email}: {Error}", request.Email, error.Description);
                return error.ToActionResult();
            });
    }

    [HttpPost("changePassword")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto request)
    {
        logger.LogInformation("Processing password change request");
        var user = await authenticationService.ChangePasswordAsync(request, AccessToken);
        return user.Match(
            success =>
            {
                logger.LogInformation("Password changed successfully");
                return Ok(new { message = "Password changed successfully" });
            },
            error =>
            {
                logger.LogError("Password change failed: {Error}", error.Description);
                return error.ToActionResult();
            });
    }

    [HttpDelete("deleteUser")]
    [Authorize]
    public async Task<IActionResult> DeleteUser()
    {
        logger.LogInformation("Processing user deleting request");
        var user = await authenticationService.DeleteUserAsync(AccessToken);
        return user.Match(
            success =>
            {
                logger.LogInformation("User deleted successfully");
                return Ok(new { message = "User deleted successfully" });
            },
            error =>
            {
                logger.LogError("User deletion failed: {Error}", error.Description);
                return error.ToActionResult();
            });
    }

    [HttpPost("refreshToken")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
    {
        logger.LogInformation("Processing token refresh request");
        try
        {
            var (newAccessToken, newRefreshToken) = await jwtTokenService.RefreshTokenAsync(
                request.AccessToken,
                request.RefreshToken);

            logger.LogInformation("Token refresh successful");
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
            logger.LogWarning("Token refresh failed: {Error}", ex.Message);
            return Unauthorized(new { message = ex.Message });
        }
    }

    [HttpGet("isAuthenticated")]
    public IActionResult IsAuthenticated()
    {
        var token = AccessToken;
        var isValid = jwtTokenService.ValidateCurrentToken(token);
        return Ok(new { isAuthenticated = isValid });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        logger.LogInformation("Processing logout request");
        var result = await authenticationService.LogoutAsync(AccessToken);

        return result.Match(
            success =>
            {
                logger.LogInformation("User logged out successfully");
                return Ok(new { message = "Logout successful" });
            },
            error =>
            {
                logger.LogError("Logout failed: {Error}", error.Description);
                return error.ToActionResult();
            });
    }
}