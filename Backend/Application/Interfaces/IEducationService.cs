using Application.DTOs.Education;
using Domain.Common;

namespace Application.Interfaces;

public interface IEducationService
{
    Task<Result<IEnumerable<EducationResponseDto>>> GetEducationsByIdAsync();
    Task<Result<bool>> AddEducationAsync(EducationRequestDto educationDto, string accessToken);
    Task<Result<bool>> UpdateEducationAsync(EducationRequestDto educationDto, Guid id, string accessToken);
    Task<Result<bool>> DeleteEducationAsync(Guid id, string accessToken);
}