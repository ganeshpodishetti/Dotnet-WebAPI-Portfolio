using API.Handlers;
using API.Helpers;
using Application.DTOs.Skill;
using Application.Interfaces;
using Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/skills")]
[Authorize(Roles = "Admin")]
public class SkillController(
    ISkillService skillService,
    IAccessTokenHelper accessTokenHelper) : Controller
{
    private string AccessToken => accessTokenHelper.GetAccessToken();

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetSkillsAsync()
    {
        var result = await skillService.GetAllSkillsByUserIdAsync(AccessToken);
        return result.Match(
            success => Ok(result.Value),
            error => error.ToActionResult());
    }

    [HttpPost]
    public async Task<IActionResult> AddSkillAsync([FromBody] SkillRequestDto request)
    {
        var result = await skillService.AddSkillAsync(request, AccessToken);
        return result.Match(
            success => Ok(new { message = "Skill added successfully" }),
            error => error.ToActionResult());
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateSkillAsync([FromBody] SkillRequestDto request, [FromRoute] Guid id)
    {
        var result = await skillService.UpdateSkillAsync(request, id, AccessToken);
        return result.Match(
            success => Ok(new { message = "Skill edited successfully" }),
            error => error.ToActionResult());
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteSkillAsync([FromRoute] Guid id)
    {
        var result = await skillService.DeleteSkillAsync(id, AccessToken);
        return result.Match(
            success => Ok(new { message = "Skill deleted successfully" }),
            error => error.ToActionResult());
    }
}