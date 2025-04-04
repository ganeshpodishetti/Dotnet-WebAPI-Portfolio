using API.Handlers;
using API.Helpers;
using Application.DTOs.SocialLink;
using Application.Interfaces;
using Domain.Common.ResultPattern;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/socialLink")]
[Authorize(Roles = nameof(UserRole.Admin))]
public class SocialLinkController(
    ISocialLinkService socialLinkService,
    IAccessTokenHelper accessTokenHelper,
    ILogger<SocialLinkController> logger) : Controller
{
    private string AccessToken => accessTokenHelper.GetAccessToken();

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetSocialLinksByUserId()
    {
        logger.LogInformation("Retrieving social links");
        var result = await socialLinkService.GetSocialLinksByUserIdAsync();
        return result.Match(
            success =>
            {
                logger.LogInformation("Successfully retrieved {Count} social links", result.Value.Count());
                return Ok(result.Value);
            },
            error =>
            {
                logger.LogError("Failed to retrieve social links: {Error}", error.Description);
                return error.ToActionResult();
            });
    }

    [HttpPost]
    public async Task<IActionResult> AddSocialLinks([FromBody] SocialLinkRequestDto request)
    {
        logger.LogInformation("Adding new social link");
        var result = await socialLinkService.AddSocialLinkAsync(request, AccessToken);
        return result.Match(
            success =>
            {
                logger.LogInformation("Successfully added social link");
                return Ok(new { message = "Social link added successfully" });
            },
            error =>
            {
                logger.LogError("Failed to add social link: {Error}", error.Description);
                return error.ToActionResult();
            });
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateSocialLinks([FromBody] SocialLinkRequestDto request, [FromRoute] Guid id)
    {
        logger.LogInformation("Updating social link with ID: {Id}", id);
        var result = await socialLinkService.UpdateSocialLinkAsync(request, id, AccessToken);
        return result.Match(
            success =>
            {
                logger.LogInformation("Successfully updated social link with ID: {Id}", id);
                return Ok(new { message = "Social link edited successfully" });
            },
            error =>
            {
                logger.LogError("Failed to update social link: {Error}", error.Description);
                return error.ToActionResult();
            });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteSocialLinks([FromRoute] Guid id)
    {
        logger.LogInformation("Deleting social link with ID: {Id}", id);
        var result = await socialLinkService.DeleteSocialLinkAsync(id, AccessToken);
        return result.Match(
            success =>
            {
                logger.LogInformation("Successfully deleted social link with ID: {Id}", id);
                return Ok(new { message = "Social link deleted successfully" });
            },
            error =>
            {
                logger.LogError("Failed to delete social link: {Error}", error.Description);
                return error.ToActionResult();
            });
    }
}