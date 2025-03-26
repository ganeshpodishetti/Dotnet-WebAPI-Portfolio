using Application.DTOs.Experience;
using Domain.Common;

namespace Application.Interfaces;

public interface IExperienceService
{
    Task<Result<IEnumerable<ExperienceResponseDto>>> GetExperiencesByUserIdAsync(string accessToken);
    Task<Result<bool>> AddExperienceAsync(ExperienceRequestDto experienceRequestDto, string accessToken);
    Task<Result<bool>> UpdateExperienceAsync(ExperienceRequestDto experienceRequestDto, Guid id, string accessToken);
    Task<Result<bool>> DeleteExperienceAsync(Guid id, string accessToken);
}