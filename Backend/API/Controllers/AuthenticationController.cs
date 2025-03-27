using API.Extensions;
using API.Helpers;
using Application.DTOs.Authentication;
using Application.Interfaces;
using Domain.Common;
using Domain.Interfaces;
using Domain.Options;
using FluentValidation;
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
    IFormatValidation formatValidation,
    IOptions<JwtTokenOptions> jwtOptions) : Controller
{
    private string AccessToken => accessTokenHelper.GetAccessToken();

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request,
        IValidator<RegisterRequestDto> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return BadRequest(formatValidation.FormatValidationErrors(validationResult));

        var response = await authenticationService.RegisterAsync(request);
        return response.Match(
            success => Ok(response.Value),
            error => error.ToActionResult()
        );
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request,
        IValidator<LoginRequestDto> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return BadRequest(formatValidation.FormatValidationErrors(validationResult));

        var response = await authenticationService.LoginAsync(request);
        return response.Match(
            success => Ok(response.Value),
            error => error.ToActionResult());
    }

    [HttpPost("changePassword")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto request,
        IValidator<ChangePasswordDto> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return BadRequest(formatValidation.FormatValidationErrors(validationResult));

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
                RefreshToken = newRefreshToken,
                AccessTokenExpirationAtUtc = DateTime.UtcNow.AddMinutes(jwtOptions.Value.AccessTokenExpirationMinutes)
                    .ToString("yyyy-MM-dd HH:mm:ss tt"),
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