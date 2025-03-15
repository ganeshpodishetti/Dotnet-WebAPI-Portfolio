using Application.DTOs.Education;

namespace Application.Interfaces;

public interface IEducationService
{
    Task<bool> AddEducationAsync(EducationRequestDto educationDto, string accessToken);
    Task<bool> UpdateEducationAsync(EducationRequestDto educationDto, string accessToken);
    Task<bool> DeleteEducationAsync(string accessToken);
    Task<EducationResponseDto?> GetEducationByIdAsync(string accessToken);
}