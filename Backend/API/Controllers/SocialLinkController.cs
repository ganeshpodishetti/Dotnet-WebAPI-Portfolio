using API.Extensions;
using API.Helpers;
using Application.DTOs.SocialLink;
using Application.Interfaces;
using Domain.Common;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/socialLink")]
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
        //return Ok(result);
        return result.Match(
            success => Ok(result.Value),
            error => error.ToActionResult());
    }

    [HttpPost]
    public async Task<IActionResult> AddSocialLinks([FromBody] SocialLinkRequestDto request,
        IValidator<SocialLinkRequestDto> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return BadRequest(formatValidation.FormatValidationErrors(validationResult));

        var result = await socialLinkService.AddSocialLinkAsync(request, AccessToken);
        //return Ok(result);
        return result.Match(
            success => Ok(new { message = "Social link added successfully" }),
            error => error.ToActionResult());
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateSocialLinks([FromBody] SocialLinkRequestDto request,
        [FromRoute] Guid id, IValidator<SocialLinkRequestDto> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return BadRequest(formatValidation.FormatValidationErrors(validationResult));

        var result = await socialLinkService.UpdateSocialLinkAsync(request, id, AccessToken);
        //return Ok(result);
        return result.Match(
            success => Ok(new { message = "Social link edited successfully" }),
            error => error.ToActionResult());
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteSocialLinks([FromRoute] Guid id)
    {
        var result = await socialLinkService.DeleteSocialLinkAsync(id, AccessToken);
        //return Ok(result);
        return result.Match(
            success => Ok(new { message = "Social link deleted successfully" }),
            error => error.ToActionResult());
    }
}