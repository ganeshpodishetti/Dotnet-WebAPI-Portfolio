using API.Helpers;
using Application.DTOs.Project;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/projects")]
[Authorize]
public class ProjectController(
    IProjectService projectService,
    IAccessTokenHelper accessTokenHelper) : Controller
{
    private string AccessToken => accessTokenHelper.GetAccessToken();

    [HttpGet]
    public async Task<IActionResult> GetProjectsByUserId()
    {
        var result = await projectService.GetProjectsByUserIdAsync(AccessToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddProject([FromBody] ProjectRequestDto projectRequestDto)
    {
        var result = await projectService.AddProjectAsync(projectRequestDto, AccessToken);
        return Ok(result);
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateProject([FromBody] ProjectRequestDto projectRequestDto,
        [FromRoute] Guid id)
    {
        var result = await projectService.UpdateProjectAsync(projectRequestDto, id, AccessToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteProject([FromRoute] Guid id)
    {
        var result = await projectService.DeleteProjectAsync(id, AccessToken);
        return Ok(result);
    }
}