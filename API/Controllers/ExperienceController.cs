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

    [HttpGet("getExperiencesById")]
    public async Task<IActionResult> GetExperiencesByUserId()
    {
        var result = await experienceService.GetExperiencesByUserIdAsync(AccessToken);
        return Ok(result);
    }

    [HttpPost("addExperience")]
    public async Task<IActionResult> AddExperience([FromBody] ExperienceRequestDto experienceRequestDto)
    {
        var result = await experienceService.AddExperienceAsync(experienceRequestDto, AccessToken);
        return Ok(result);
    }

    [HttpPatch("updateExperience")]
    public async Task<IActionResult> UpdateExperienceAsync([FromBody] ExperienceRequestDto experienceRequestDto)
    {
        var result = await experienceService.UpdateExperienceAsync(experienceRequestDto, AccessToken);
        return Ok(result);
    }

    [HttpDelete("deleteExperience")]
    public async Task<IActionResult> DeleteExperienceAsync()
    {
        var result = await experienceService.DeleteExperienceAsync(AccessToken);
        return Ok(result);
    }
}