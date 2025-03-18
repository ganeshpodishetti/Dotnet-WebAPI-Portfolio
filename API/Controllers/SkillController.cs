using API.Helpers;
using Application.DTOs.Skill;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/skills")]
[Authorize]
public class SkillController(
    ISkillService skillService,
    IAccessTokenHelper accessTokenHelper) : Controller
{
    private string AccessToken => accessTokenHelper.GetAccessToken();

    [HttpGet]
    public async Task<IActionResult> GetSkillsAsync()
    {
        var result = await skillService.GetAllSkillsByUserIdAsync(AccessToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddSkillAsync([FromBody] SkillRequestDto skillDto)
    {
        var result = await skillService.AddSkillAsync(skillDto, AccessToken);
        return Ok(result);
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> UpdateSkillAsync([FromBody] SkillRequestDto skillDto, [FromRoute] Guid id)
    {
        var result = await skillService.UpdateSkillAsync(skillDto, id, AccessToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteSkillAsync([FromRoute] Guid id)
    {
        var result = await skillService.DeleteSkillAsync(id, AccessToken);
        return Ok(result);
    }
}