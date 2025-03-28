using API.Handlers;
using API.Helpers;
using Application.DTOs.Education;
using Application.Interfaces;
using Domain.Common.ResultPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/education")]
[Authorize(Roles = "Admin")]
public class EducationController(
    IEducationService educationService,
    IAccessTokenHelper accessTokenHelper,
    ILogger<EducationController> logger) : Controller
{
    private string AccessToken => accessTokenHelper.GetAccessToken();

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetEducationByIdAsync()
    {
        logger.LogInformation("Retrieving education records");
        var result = await educationService.GetEducationsByIdAsync();
        return result.Match(
            success =>
            {
                logger.LogInformation("Successfully retrieved {Count} education records", result.Value.Count());
                return Ok(result.Value);
            },
            error =>
            {
                logger.LogError("Failed to retrieve education records: {Error}", error.Description);
                return error.ToActionResult();
            });
    }

    [HttpPost]
    public async Task<IActionResult> AddEducationAsync([FromBody] EducationRequestDto request)
    {
        logger.LogInformation("Adding new education record: {School}", request.School);
        var result = await educationService.AddEducationAsync(request, AccessToken);
        return result.Match(
            success =>
            {
                logger.LogInformation("Successfully added education record: {School}", request.School);
                return Ok(new { message = "Education added successfully" });
            },
            error =>
            {
                logger.LogError("Failed to add education record: {Error}", error.Description);
                return error.ToActionResult();
            });
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateEducationAsync([FromBody] EducationRequestDto request,
        [FromRoute] Guid id)
    {
        logger.LogInformation("Updating education record with ID: {Id}", id);
        var result = await educationService.UpdateEducationAsync(request, id, AccessToken);
        return result.Match(
            success =>
            {
                logger.LogInformation("Successfully updated education record with ID: {Id}", id);
                return Ok(new { message = "Education edited successfully" });
            },
            error =>
            {
                logger.LogError("Failed to update education record: {Error}", error.Description);
                return error.ToActionResult();
            });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteEducationAsync([FromRoute] Guid id)
    {
        logger.LogInformation("Deleting education record with ID: {Id}", id);
        var result = await educationService.DeleteEducationAsync(id, AccessToken);
        return result.Match(
            success =>
            {
                logger.LogInformation("Successfully deleted education record with ID: {Id}", id);
                return Ok(new { message = "Education deleted successfully" });
            },
            error =>
            {
                logger.LogError("Failed to delete education record: {Error}", error.Description);
                return error.ToActionResult();
            });
    }
}