using API.Extensions;
using API.Helpers;
using Application.DTOs.Skill;
using Application.Interfaces;
using Domain.Common;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/skills")]
[Authorize]
public class SkillController(
    ISkillService skillService,
    IAccessTokenHelper accessTokenHelper,
    IFormatValidation formatValidation) : Controller
{
    private string AccessToken => accessTokenHelper.GetAccessToken();

    [HttpGet]
    public async Task<IActionResult> GetSkillsAsync()
    {
        var result = await skillService.GetAllSkillsByUserIdAsync(AccessToken);
        //return Ok(result);
        return result.Match(
            success => Ok(result.Value),
            error => error.ToActionResult());
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddSkillAsync([FromBody] SkillRequestDto request,
        IValidator<SkillRequestDto> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return BadRequest(formatValidation.FormatValidationErrors(validationResult));

        var result = await skillService.AddSkillAsync(request, AccessToken);
        //return Ok(result);
        return result.Match(
            success => Ok(new { message = "Skill added successfully" }),
            error => error.ToActionResult());
    }

    [HttpPatch("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateSkillAsync([FromBody] SkillRequestDto request, [FromRoute] Guid id,
        IValidator<SkillRequestDto> validator)
    {
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return BadRequest(formatValidation.FormatValidationErrors(validationResult));

        var result = await skillService.UpdateSkillAsync(request, id, AccessToken);
        //return Ok(result);
        return result.Match(
            success => Ok(new { message = "Skill edited successfully" }),
            error => error.ToActionResult());
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteSkillAsync([FromRoute] Guid id)
    {
        var result = await skillService.DeleteSkillAsync(id, AccessToken);
        //return Ok(result);
        return result.Match(
            success => Ok(new { message = "Skill deleted successfully" }),
            error => error.ToActionResult());
    }
}