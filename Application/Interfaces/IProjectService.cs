using Application.DTOs.Project;

namespace Application.Interfaces;

public interface IProjectService
{
    Task<IEnumerable<ProjectResponseDto>> GetProjectsByUserIdAsync(string accessToken);
    Task<bool> AddProjectAsync(ProjectRequestDto experienceRequestDto, string accessToken);
    Task<bool> UpdateProjectAsync(ProjectRequestDto experienceRequestDto, string accessToken);
    Task<bool> DeleteProjectAsync(string accessToken);
}