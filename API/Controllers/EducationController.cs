using API.Helpers;
using Application.DTOs.Education;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/education")]
[Authorize]
public class EducationController(
    IEducationService educationService,
    IAccessTokenHelper accessTokenHelper) : Controller
{
    private string AccessToken => accessTokenHelper.GetAccessToken();

    [HttpPost("addEducation")]
    public async Task<IActionResult> AddEducationAsync([FromBody] EducationRequestDto educationDto)
    {
        var result = await educationService.AddEducationAsync(educationDto, AccessToken);
        return Ok(result);
    }

    [HttpPatch("updateEducation")]
    public async Task<IActionResult> UpdateEducationAsync([FromBody] EducationRequestDto educationDto)
    {
        var result = await educationService.UpdateEducationAsync(educationDto, AccessToken);
        return Ok(result);
    }

    [HttpDelete("deleteEducation")]
    public async Task<IActionResult> DeleteEducationAsync(string schoolName)
    {
        var result = await educationService.DeleteEducationAsync(AccessToken);
        return Ok(result);
    }

    [HttpGet("getEducationById")]
    public async Task<IActionResult> GetEducationByIdAsync()
    {
        var result = await educationService.GetEducationsByIdAsync(AccessToken);
        return Ok(result);
    }
}