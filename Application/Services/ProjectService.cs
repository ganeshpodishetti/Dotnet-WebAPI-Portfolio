using Application.DTOs.Project;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.UnitOfWork;

namespace Application.Services;

public class ProjectService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IJwtTokenService jwtTokenService) : IProjectService
{
    // get projects
    public async Task<IEnumerable<ProjectResponseDto>> GetProjectsByUserIdAsync(string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);

        var projects = await unitOfWork.ProjectRepository.GetAllByUserIdAsync(userId);
        var projectWithSkills = mapper.Map<IEnumerable<ProjectResponseDto>>(projects);

        return projectWithSkills;
    }

    // add projects
    public async Task<bool> AddProjectAsync(ProjectRequestDto projectRequestDto, string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);

        var existingUser = await unitOfWork.UserRepository.GetByIdAsync(userId);
        if (existingUser is null)
            throw new Exception("User does not exist to add experience");

        var project = mapper.Map<Project>(projectRequestDto);
        project.UserId = existingUser.Id;
        project.UpdatedAt = DateTime.UtcNow;

        var result = await unitOfWork.ProjectRepository.AddAsync(project);
        await unitOfWork.CommitAsync();
        return result;
    }

    // edit projects
    public async Task<bool> UpdateProjectAsync(ProjectRequestDto experienceRequestDto, Guid projectId,
        string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        var existingProject = await unitOfWork.ProjectRepository.GetByUserIdAsync(userId, projectId);
        if (existingProject is null)
            throw new Exception("User does not exist to update experience.");

        // Map DTO to existing entity to preserve Id
        mapper.Map(experienceRequestDto, existingProject);
        existingProject.UpdatedAt = DateTime.UtcNow;

        var result = await unitOfWork.ProjectRepository.UpdateAsync(existingProject);
        await unitOfWork.CommitAsync();
        return result;
    }

    // delete projects
    public async Task<bool> DeleteProjectAsync(Guid projectId, string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        var existingProject = await unitOfWork.ProjectRepository.GetByUserIdAsync(userId, projectId);
        if (existingProject is null)
            throw new Exception("User does not exist to delete experience.");

        var result = await unitOfWork.ProjectRepository.DeleteAsync(existingProject);
        await unitOfWork.CommitAsync();
        return result;
    }
}