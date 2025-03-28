using Application.DTOs.Skill;
using Domain.Common.ResultPattern;

namespace Application.Interfaces;

public interface ISkillService
{
    Task<Result<IEnumerable<SkillResponseDto>>> GetAllSkillsByUserIdAsync(string accessToken);
    Task<Result<bool>> AddSkillAsync(SkillRequestDto skill, string accessToken);
    Task<Result<bool>> UpdateSkillAsync(SkillRequestDto skill, Guid id, string accessToken);
    Task<Result<bool>> DeleteSkillAsync(Guid id, string accessToken);
}