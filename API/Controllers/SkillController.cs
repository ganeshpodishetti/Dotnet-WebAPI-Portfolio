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

    [HttpGet("getSkills")]
    public async Task<IActionResult> GetSkillsAsync()
    {
        var result = await skillService.GetAllSkillsByUserIdAsync(AccessToken);
        return Ok(result);
    }

    [HttpPost("addSkill")]
    public async Task<IActionResult> AddSkillAsync([FromBody] SkillRequestDto skillDto)
    {
        var result = await skillService.AddSkillAsync(skillDto, AccessToken);
        return Ok(result);
    }

    [HttpPatch("updateSkill")]
    public async Task<IActionResult> UpdateSkillAsync([FromBody] SkillRequestDto skillDto)
    {
        var result = await skillService.UpdateSkillAsync(skillDto, AccessToken);
        return Ok(result);
    }

    [HttpDelete("deleteSkill")]
    public async Task<IActionResult> DeleteSkillAsync()
    {
        var result = await skillService.DeleteSkillAsync(AccessToken);
        return Ok(result);
    }
}