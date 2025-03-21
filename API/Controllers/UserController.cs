using API.Helpers;
using Application.DTOs.User;
using Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/user")]
[Authorize]
public class UserController(
    IUserServices userServices,
    IAccessTokenHelper accessTokenHelper,
    IFormatValidation formatValidation) : Controller
{
    private string AccessToken => accessTokenHelper.GetAccessToken();

    // GET: api/user/GetUserProfiles/id
    [HttpGet]
    public async Task<IActionResult> GetProfileById()
    {
        var result = await userServices.GetProfileByIdAsync(AccessToken);
        return Ok(result);
    }

    // PUT: api/user/UpdateUserProfile
    [HttpPatch]
    public async Task<IActionResult> UpdateUserProfile([FromBody] UserRequestDto request,
        IValidator<UserRequestDto> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return BadRequest(formatValidation.FormatValidationErrors(validationResult));

        var result = await userServices.UpdateProfileAsync(request, AccessToken);
        return Ok(result);
    }
}