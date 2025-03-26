using API.Extensions;
using API.Helpers;
using Application.DTOs.Education;
using Application.Interfaces;
using Domain.Common;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/education")]
[Authorize]
public class EducationController(
    IEducationService educationService,
    IAccessTokenHelper accessTokenHelper,
    IFormatValidation formatValidation) : Controller
{
    private string AccessToken => accessTokenHelper.GetAccessToken();

    [HttpGet]
    public async Task<IActionResult> GetEducationByIdAsync()
    {
        var result = await educationService.GetEducationsByIdAsync(AccessToken);
        return result.Match(
            success => Ok(result.Value),
            error => error.ToActionResult());
    }

    [HttpPost]
    public async Task<IActionResult> AddEducationAsync([FromBody] EducationRequestDto request,
        IValidator<EducationRequestDto> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return BadRequest(formatValidation.FormatValidationErrors(validationResult));

        var result = await educationService.AddEducationAsync(request, AccessToken);
        return result.Match(
            success => Ok(new { message = "Education added successfully" }),
            error => error.ToActionResult());
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateEducationAsync([FromBody] EducationRequestDto request,
        [FromRoute] Guid id, IValidator<EducationRequestDto> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return BadRequest(formatValidation.FormatValidationErrors(validationResult));

        var result = await educationService.UpdateEducationAsync(request, id, AccessToken);
        return result.Match(
            success => Ok(new { message = "Education edited successfully" }),
            error => error.ToActionResult());
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteEducationAsync([FromRoute] Guid id)
    {
        var result = await educationService.DeleteEducationAsync(id, AccessToken);
        return result.Match(
            success => Ok(new { message = "Education deleted successfully" }),
            error => error.ToActionResult());
    }
}