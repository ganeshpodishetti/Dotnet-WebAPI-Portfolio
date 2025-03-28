using API.Handlers;
using API.Helpers;
using Application.DTOs.Skill;
using Application.Interfaces;
using Domain.Common.ResultPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/skills")]
[Authorize(Roles = "Admin")]
public class SkillController(
    ISkillService skillService,
    IAccessTokenHelper accessTokenHelper,
    ILogger<SkillController> logger) : Controller
{
    private string AccessToken => accessTokenHelper.GetAccessToken();

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetSkillsAsync()
    {
        logger.LogInformation("Retrieving skills");
        var result = await skillService.GetAllSkillsByUserIdAsync();
        return result.Match(
            success =>
            {
                logger.LogInformation("Successfully retrieved {Count} skills", result.Value.Count());
                return Ok(result.Value);
            },
            error =>
            {
                logger.LogError("Failed to retrieve skills: {Error}", error.Description);
                return error.ToActionResult();
            });
    }

    [HttpPost]
    public async Task<IActionResult> AddSkillAsync([FromBody] SkillRequestDto request)
    {
        logger.LogInformation("Adding new skill: {Category}", request.SkillCategory);
        var result = await skillService.AddSkillAsync(request, AccessToken);
        return result.Match(
            success =>
            {
                logger.LogInformation("Successfully added skill: {Category}", request.SkillCategory);
                return Ok(new { message = "Skill added successfully" });
            },
            error =>
            {
                logger.LogError("Failed to add skill: {Error}", error.Description);
                return error.ToActionResult();
            });
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateSkillAsync([FromBody] SkillRequestDto request, [FromRoute] Guid id)
    {
        logger.LogInformation("Updating skill with ID: {Id}", id);
        var result = await skillService.UpdateSkillAsync(request, id, AccessToken);
        return result.Match(
            success =>
            {
                logger.LogInformation("Successfully updated skill with ID: {Id}", id);
                return Ok(new { message = "Skill edited successfully" });
            },
            error =>
            {
                logger.LogError("Failed to update skill: {Error}", error.Description);
                return error.ToActionResult();
            });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteSkillAsync([FromRoute] Guid id)
    {
        logger.LogInformation("Deleting skill with ID: {Id}", id);
        var result = await skillService.DeleteSkillAsync(id, AccessToken);
        return result.Match(
            success =>
            {
                logger.LogInformation("Successfully deleted skill with ID: {Id}", id);
                return Ok(new { message = "Skill deleted successfully" });
            },
            error =>
            {
                logger.LogError("Failed to delete skill: {Error}", error.Description);
                return error.ToActionResult();
            });
    }
}