using API.Extensions;
using API.Helpers;
using Application.DTOs.Authentication;
using Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/authentication")]
public class AuthenticationController(
    IAuthenticationService authenticationService,
    IAccessTokenHelper accessTokenHelper,
    IFormatValidation formatValidation) : Controller
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
        return response.Match<IActionResult>(
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
        return response.Match<IActionResult>(
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
        return user.Match<IActionResult>(
            success => Ok(new { message = "Password changed successfully" }),
            error => error.ToActionResult());
    }

    [HttpDelete("deleteUser")]
    [Authorize]
    public async Task<IActionResult> DeleteUser()
    {
        var user = await authenticationService.DeleteUserAsync(AccessToken);
        return user.Match<IActionResult>(
            success => Ok(new { message = "User deleted successfully" }),
            error => error.ToActionResult());
    }
}