using API.Extensions;
using API.Helpers;
using Application.DTOs.Experience;
using Application.Interfaces;
using Domain.Common;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/experience")]
[Authorize]
public class ExperienceController(
    IExperienceService experienceService,
    IAccessTokenHelper accessTokenHelper,
    IFormatValidation formatValidation) : Controller
{
    private string AccessToken => accessTokenHelper.GetAccessToken();

    [HttpGet]
    public async Task<IActionResult> GetExperiencesByUserId()
    {
        var result = await experienceService.GetExperiencesByUserIdAsync(AccessToken);
        //return Ok(result);
        return result.Match(
            success => Ok(result.Value),
            error => error.ToActionResult());
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddExperience([FromBody] ExperienceRequestDto request,
        IValidator<ExperienceRequestDto> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return BadRequest(formatValidation.FormatValidationErrors(validationResult));
        var result = await experienceService.AddExperienceAsync(request, AccessToken);
        //return Ok(result);
        return result.Match(
            success => Ok(new { message = "Experience added successfully" }),
            error => error.ToActionResult());
    }

    [HttpPatch("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateExperienceAsync([FromBody] ExperienceRequestDto request,
        [FromRoute] Guid id, IValidator<ExperienceRequestDto> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return BadRequest(formatValidation.FormatValidationErrors(validationResult));

        var result = await experienceService.UpdateExperienceAsync(request, id, AccessToken);
        //return Ok(result);
        return result.Match(
            success => Ok(new { message = "Experience edited successfully" }),
            error => error.ToActionResult());
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteExperienceAsync([FromRoute] Guid id)
    {
        var result = await experienceService.DeleteExperienceAsync(id, AccessToken);
        //return Ok(result);
        return result.Match(
            success => Ok(new { message = "Experience deleted successfully" }),
            error => error.ToActionResult());
    }
}