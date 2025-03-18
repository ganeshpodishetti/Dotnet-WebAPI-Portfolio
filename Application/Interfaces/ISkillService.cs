using Application.DTOs.Skill;

namespace Application.Interfaces;

public interface ISkillService
{
    Task<List<SkillResponseDto>> GetAllSkillsByUserIdAsync(string accessToken);
    Task<bool> AddSkillAsync(SkillRequestDto skill, string accessToken);
    Task<bool> UpdateSkillAsync(SkillRequestDto skill, string accessToken);
    Task<bool> DeleteSkillAsync(string accessToken);
}