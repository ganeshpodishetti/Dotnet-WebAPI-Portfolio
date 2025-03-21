using API.Helpers;
using Application.DTOs.SocialLink;
using Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/social-link")]
[Authorize]
public class SocialLinkController(
    ISocialLinkService socialLinkService,
    IAccessTokenHelper accessTokenHelper,
    IFormatValidation formatValidation)
    : Controller
{
    private string AccessToken => accessTokenHelper.GetAccessToken();

    [HttpGet]
    public async Task<IActionResult> GetSocialLinksByUserId()
    {
        var result = await socialLinkService.GetSocialLinksByUserIdAsync(AccessToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddSocialLinks([FromBody] SocialLinkRequestDto request,
        IValidator<SocialLinkRequestDto> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return BadRequest(formatValidation.FormatValidationErrors(validationResult));

        var result = await socialLinkService.AddSocialLinkAsync(request, AccessToken);
        return Ok(result);
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateSocialLinks([FromBody] SocialLinkRequestDto request,
        [FromRoute] Guid id, IValidator<SocialLinkRequestDto> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return BadRequest(formatValidation.FormatValidationErrors(validationResult));

        var result = await socialLinkService.UpdateSocialLinkAsync(request, id, AccessToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteSocialLinks([FromRoute] Guid id)
    {
        var result = await socialLinkService.DeleteSocialLinkAsync(id, AccessToken);
        return Ok(result);
    }
}