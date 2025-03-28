using API.Handlers;
using API.Helpers;
using Application.DTOs.Project;
using Application.Interfaces;
using Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/projects")]
[Authorize(Roles = "Admin")]
public class ProjectController(
    IProjectService projectService,
    IAccessTokenHelper accessTokenHelper) : Controller
{
    private string AccessToken => accessTokenHelper.GetAccessToken();

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetProjectsByUserId()
    {
        var result = await projectService.GetProjectsByUserIdAsync(AccessToken);
        return result.Match(
            success => Ok(result.Value),
            error => error.ToActionResult());
    }

    [HttpPost]
    public async Task<IActionResult> AddProject([FromBody] ProjectRequestDto request)
    {
        var result = await projectService.AddProjectAsync(request, AccessToken);
        return result.Match(
            success => Ok(new { message = "Project added successfully" }),
            error => error.ToActionResult());
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateProject([FromBody] ProjectRequestDto request,
        [FromRoute] Guid id)
    {
        var result = await projectService.UpdateProjectAsync(request, id, AccessToken);
        return result.Match(
            success => Ok(new { message = "Project edited successfully" }),
            error => error.ToActionResult());
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteProject([FromRoute] Guid id)
    {
        var result = await projectService.DeleteProjectAsync(id, AccessToken);
        return result.Match(
            success => Ok(new { message = "Project deleted successfully" }),
            error => error.ToActionResult());
    }
}