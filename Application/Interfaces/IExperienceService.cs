using Application.DTOs.Experience;

namespace Application.Interfaces;

public interface IExperienceService
{
    Task<List<ExperienceResponseDto>> GetExperiencesByUserIdAsync(string accessToken);
    Task<bool> AddExperienceAsync(ExperienceRequestDto experienceRequestDto, string accessToken);
    Task<bool> UpdateExperienceAsync(ExperienceRequestDto experienceRequestDto, string accessToken);
    Task<bool> DeleteExperienceAsync(string accessToken);
}