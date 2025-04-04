using API.Handlers;
using API.Helpers;
using Application.DTOs.Experience;
using Application.Interfaces;
using Domain.Common.ResultPattern;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/experience")]
[Authorize(Roles = nameof(UserRole.Admin))]
public class ExperienceController(
    IExperienceService experienceService,
    IAccessTokenHelper accessTokenHelper,
    ILogger<ExperienceController> logger) : Controller
{
    private string AccessToken => accessTokenHelper.GetAccessToken();

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetExperiencesByUserId()
    {
        logger.LogInformation("Retrieving experiences");
        var result = await experienceService.GetExperiencesByUserIdAsync();
        return result.Match(
            success =>
            {
                logger.LogInformation("Successfully retrieved {Count} experiences", result.Value.Count());
                return Ok(result.Value);
            },
            error =>
            {
                logger.LogError("Failed to retrieve experiences: {Error}", error.Description);
                return error.ToActionResult();
            });
    }

    [HttpPost]
    public async Task<IActionResult> AddExperience([FromBody] ExperienceRequestDto request)
    {
        logger.LogInformation("Adding new experience at {Company}", request.CompanyName);
        var result = await experienceService.AddExperienceAsync(request, AccessToken);
        return result.Match(
            success =>
            {
                logger.LogInformation("Successfully added experience at {Company}", request.CompanyName);
                return Ok(new { message = "Experience added successfully" });
            },
            error =>
            {
                logger.LogError("Failed to add experience: {Error}", error.Description);
                return error.ToActionResult();
            });
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateExperienceAsync([FromBody] ExperienceRequestDto request, [FromRoute] Guid id)
    {
        logger.LogInformation("Updating experience with ID: {Id}", id);
        var result = await experienceService.UpdateExperienceAsync(request, id, AccessToken);
        return result.Match(
            success =>
            {
                logger.LogInformation("Successfully updated experience with ID: {Id}", id);
                return Ok(new { message = "Experience edited successfully" });
            },
            error =>
            {
                logger.LogError("Failed to update experience: {Error}", error.Description);
                return error.ToActionResult();
            });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteExperienceAsync([FromRoute] Guid id)
    {
        logger.LogInformation("Deleting experience with ID: {Id}", id);
        var result = await experienceService.DeleteExperienceAsync(id, AccessToken);
        return result.Match(
            success =>
            {
                logger.LogInformation("Successfully deleted experience with ID: {Id}", id);
                return Ok(new { message = "Experience deleted successfully" });
            },
            error =>
            {
                logger.LogError("Failed to delete experience: {Error}", error.Description);
                return error.ToActionResult();
            });
    }
}