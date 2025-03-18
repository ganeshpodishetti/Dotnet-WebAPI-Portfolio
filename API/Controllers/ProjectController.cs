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

    [HttpGet("getProjectsByUserId")]
    public async Task<IActionResult> GetProjectsByUserId()
    {
        var result = await projectService.GetProjectsByUserIdAsync(AccessToken);
        return Ok(result);
    }

    [HttpPost("addProject")]
    public async Task<IActionResult> AddProject([FromBody] ProjectRequestDto projectRequestDto)
    {
        var result = await projectService.AddProjectAsync(projectRequestDto, AccessToken);
        return Ok(result);
    }

    [HttpPatch("updateProject")]
    public async Task<IActionResult> UpdateProject([FromBody] ProjectRequestDto projectRequestDto)
    {
        var result = await projectService.UpdateProjectAsync(projectRequestDto, AccessToken);
        return Ok(result);
    }

    [HttpDelete("deleteProject")]
    public async Task<IActionResult> DeleteProject()
    {
        var result = await projectService.DeleteProjectAsync(AccessToken);
        return Ok(result);
    }
}