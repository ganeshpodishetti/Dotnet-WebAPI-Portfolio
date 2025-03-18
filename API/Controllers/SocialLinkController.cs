using API.Helpers;
using Application.DTOs.SocialLink;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/social-link")]
[Authorize]
public class SocialLinkController(
    ISocialLinkService socialLinkService,
    IAccessTokenHelper accessTokenHelper)
    : Controller
{
    private string AccessToken => accessTokenHelper.GetAccessToken();

    [HttpGet("getSocialLinks")]
    public async Task<IActionResult> GetSocialLinksByUserId()
    {
        var result = await socialLinkService.GetSocialLinksByUserIdAsync(AccessToken);
        return Ok(result);
    }

    [HttpPost("addSocialLinks")]
    public async Task<IActionResult> AddSocialLinks([FromBody] SocialLinkRequestDto socialLinks)
    {
        var result = await socialLinkService.AddSocialLinkAsync(socialLinks, AccessToken);
        return Ok(result);
    }

    [HttpPatch("updateSocialLinks")]
    public async Task<IActionResult> UpdateSocialLinks([FromBody] SocialLinkRequestDto socialLinks)
    {
        var result = await socialLinkService.UpdateSocialLinkAsync(socialLinks, AccessToken);
        return Ok(result);
    }

    [HttpDelete("deleteSocialLinks")]
    public async Task<IActionResult> DeleteSocialLinks()
    {
        var result = await socialLinkService.DeleteSocialLinkAsync(AccessToken);
        return Ok(result);
    }
}