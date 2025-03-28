using API.Handlers;
using API.Helpers;
using Application.DTOs.SocialLink;
using Application.Interfaces;
using Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/socialLink")]
[Authorize(Roles = "Admin")]
public class SocialLinkController(
    ISocialLinkService socialLinkService,
    IAccessTokenHelper accessTokenHelper) : Controller
{
    private string AccessToken => accessTokenHelper.GetAccessToken();

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetSocialLinksByUserId()
    {
        var result = await socialLinkService.GetSocialLinksByUserIdAsync(AccessToken);
        return result.Match(
            success => Ok(result.Value),
            error => error.ToActionResult());
    }

    [HttpPost]
    public async Task<IActionResult> AddSocialLinks([FromBody] SocialLinkRequestDto request)
    {
        var result = await socialLinkService.AddSocialLinkAsync(request, AccessToken);
        return result.Match(
            success => Ok(new { message = "Social link added successfully" }),
            error => error.ToActionResult());
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateSocialLinks([FromBody] SocialLinkRequestDto request,
        [FromRoute] Guid id)
    {
        var result = await socialLinkService.UpdateSocialLinkAsync(request, id, AccessToken);
        return result.Match(
            success => Ok(new { message = "Social link edited successfully" }),
            error => error.ToActionResult());
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteSocialLinks([FromRoute] Guid id)
    {
        var result = await socialLinkService.DeleteSocialLinkAsync(id, AccessToken);
        return result.Match(
            success => Ok(new { message = "Social link deleted successfully" }),
            error => error.ToActionResult());
    }
}