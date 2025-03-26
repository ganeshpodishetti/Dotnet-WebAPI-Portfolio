using Application.DTOs.Project;
using Domain.Common;

namespace Application.Interfaces;

public interface IProjectService
{
    Task<Result<IEnumerable<ProjectResponseDto>>> GetProjectsByUserIdAsync(string accessToken);
    Task<Result<bool>> AddProjectAsync(ProjectRequestDto experienceRequestDto, string accessToken);
    Task<Result<bool>> UpdateProjectAsync(ProjectRequestDto experienceRequestDto, Guid id, string accessToken);
    Task<Result<bool>> DeleteProjectAsync(Guid id, string accessToken);
}