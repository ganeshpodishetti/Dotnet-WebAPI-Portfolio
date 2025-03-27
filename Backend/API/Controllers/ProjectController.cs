using API.Extensions;
using API.Helpers;
using Application.DTOs.Project;
using Application.Interfaces;
using Domain.Common;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/projects")]
[Authorize]
public class ProjectController(
    IProjectService projectService,
    IAccessTokenHelper accessTokenHelper,
    IFormatValidation formatValidation) : Controller
{
    private string AccessToken => accessTokenHelper.GetAccessToken();

    [HttpGet]
    public async Task<IActionResult> GetProjectsByUserId()
    {
        var result = await projectService.GetProjectsByUserIdAsync(AccessToken);
        //return Ok(result);
        return result.Match(
            success => Ok(result.Value),
            error => error.ToActionResult());
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddProject([FromBody] ProjectRequestDto request,
        IValidator<ProjectRequestDto> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return BadRequest(formatValidation.FormatValidationErrors(validationResult));

        var result = await projectService.AddProjectAsync(request, AccessToken);
        //return Ok(result);
        return result.Match(
            success => Ok(new { message = "Project added successfully" }),
            error => error.ToActionResult());
    }

    [HttpPatch("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateProject([FromBody] ProjectRequestDto request,
        [FromRoute] Guid id, IValidator<ProjectRequestDto> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return BadRequest(formatValidation.FormatValidationErrors(validationResult));

        var result = await projectService.UpdateProjectAsync(request, id, AccessToken);
        //return Ok(result);
        return result.Match(
            success => Ok(new { message = "Project edited successfully" }),
            error => error.ToActionResult());
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteProject([FromRoute] Guid id)
    {
        var result = await projectService.DeleteProjectAsync(id, AccessToken);
        //return Ok(result);
        return result.Match(
            success => Ok(new { message = "Project deleted successfully" }),
            error => error.ToActionResult());
    }
}