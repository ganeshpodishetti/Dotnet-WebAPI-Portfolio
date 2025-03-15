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
        if (result is false) return BadRequest("Failed to add education");
        return Ok("Education added successfully");
    }

    [HttpPatch("updateEducation")]
    public async Task<IActionResult> UpdateEducationAsync([FromBody] EducationRequestDto educationDto)
    {
        var result = await educationService.UpdateEducationAsync(educationDto, AccessToken);
        if (result is false) return BadRequest("Failed to update education");
        return Ok("Education updated successfully");
    }

    [HttpDelete("deleteEducation")]
    public async Task<IActionResult> DeleteEducationAsync()
    {
        var result = await educationService.DeleteEducationAsync(AccessToken);
        if (result is false) return BadRequest("Failed to delete education");
        return Ok("Education deleted successfully");
    }

    [HttpGet("getEducationById")]
    public async Task<IActionResult> GetEducationByIdAsync()
    {
        var result = await educationService.GetEducationByIdAsync(AccessToken);
        return Ok(result);
    }
}