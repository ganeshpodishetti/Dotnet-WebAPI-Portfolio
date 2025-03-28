using API.Handlers;
using API.Helpers;
using Application.DTOs.Experience;
using Application.Interfaces;
using Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/experience")]
[Authorize(Roles = "Admin")]
public class ExperienceController(
    IExperienceService experienceService,
    IAccessTokenHelper accessTokenHelper) : Controller
{
    private string AccessToken => accessTokenHelper.GetAccessToken();

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetExperiencesByUserId()
    {
        var result = await experienceService.GetExperiencesByUserIdAsync(AccessToken);
        return result.Match(
            success => Ok(result.Value),
            error => error.ToActionResult());
    }

    [HttpPost]
    public async Task<IActionResult> AddExperience([FromBody] ExperienceRequestDto request)
    {
        var result = await experienceService.AddExperienceAsync(request, AccessToken);
        return result.Match(
            success => Ok(new { message = "Experience added successfully" }),
            error => error.ToActionResult());
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateExperienceAsync([FromBody] ExperienceRequestDto request,
        [FromRoute] Guid id)
    {
        var result = await experienceService.UpdateExperienceAsync(request, id, AccessToken);
        return result.Match(
            success => Ok(new { message = "Experience edited successfully" }),
            error => error.ToActionResult());
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteExperienceAsync([FromRoute] Guid id)
    {
        var result = await experienceService.DeleteExperienceAsync(id, AccessToken);
        return result.Match(
            success => Ok(new { message = "Experience deleted successfully" }),
            error => error.ToActionResult());
    }
}