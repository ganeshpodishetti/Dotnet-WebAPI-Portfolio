using API.Handlers;
using API.Helpers;
using Application.DTOs.Project;
using Application.Interfaces;
using Domain.Common.ResultPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/projects")]
[Authorize(Roles = "Admin")]
public class ProjectController(
    IProjectService projectService,
    IAccessTokenHelper accessTokenHelper,
    ILogger<ProjectController> logger) : Controller
{
    private string AccessToken => accessTokenHelper.GetAccessToken();

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetProjectsByUserId()
    {
        logger.LogInformation("Retrieving projects for user");
        var result = await projectService.GetProjectsByUserIdAsync();
        return result.Match(
            success =>
            {
                logger.LogInformation("Successfully retrieved {Count} projects", result.Value.Count());
                return Ok(result.Value);
            },
            error =>
            {
                logger.LogError("Failed to retrieve projects: {Error}", error.Description);
                return error.ToActionResult();
            });
    }

    [HttpPost]
    public async Task<IActionResult> AddProject([FromBody] ProjectRequestDto request)
    {
        logger.LogInformation("Adding new project: {Title}", request.Name);
        var result = await projectService.AddProjectAsync(request, AccessToken);
        return result.Match(
            success =>
            {
                logger.LogInformation("Successfully added project: {Title}", request.Name);
                return Ok(new { message = "Project added successfully" });
            },
            error =>
            {
                logger.LogError("Failed to add project: {Error}", error.Description);
                return error.ToActionResult();
            });
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateProject([FromBody] ProjectRequestDto request,
        [FromRoute] Guid id)
    {
        logger.LogInformation("Updating project with ID: {Id}", id);
        var result = await projectService.UpdateProjectAsync(request, id, AccessToken);
        return result.Match(
            success =>
            {
                logger.LogInformation("Successfully updated project with ID: {Id}", id);
                return Ok(new { message = "Project edited successfully" });
            },
            error =>
            {
                logger.LogError("Failed to update project: {Error}", error.Description);
                return error.ToActionResult();
            });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteProject([FromRoute] Guid id)
    {
        logger.LogInformation("Deleting project with ID: {Id}", id);
        var result = await projectService.DeleteProjectAsync(id, AccessToken);
        return result.Match(
            success =>
            {
                logger.LogInformation("Successfully deleted project with ID: {Id}", id);
                return Ok(new { message = "Project deleted successfully" });
            },
            error =>
            {
                logger.LogError("Failed to delete project: {Error}", error.Description);
                return error.ToActionResult();
            });
    }
}