using API.Helpers;
using Application.DTOs.Experience;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/experience")]
[Authorize]
public class ExperienceController(
    IExperienceService experienceService,
    IAccessTokenHelper accessTokenHelper) : Controller
{
    private string AccessToken => accessTokenHelper.GetAccessToken();

    [HttpGet]
    public async Task<IActionResult> GetExperiencesByUserId()
    {
        var result = await experienceService.GetExperiencesByUserIdAsync(AccessToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddExperience([FromBody] ExperienceRequestDto experienceRequestDto)
    {
        var result = await experienceService.AddExperienceAsync(experienceRequestDto, AccessToken);
        return Ok(result);
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateExperienceAsync([FromBody] ExperienceRequestDto experienceRequestDto,
        [FromRoute] Guid id)
    {
        var result = await experienceService.UpdateExperienceAsync(experienceRequestDto, id, AccessToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteExperienceAsync([FromRoute] Guid id)
    {
        var result = await experienceService.DeleteExperienceAsync(id, AccessToken);
        return Ok(result);
    }
}