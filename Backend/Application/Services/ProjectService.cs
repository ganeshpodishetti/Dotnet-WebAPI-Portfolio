using Application.DTOs.Project;
using Application.Interfaces;
using AutoMapper;
using Domain.Common.ResultPattern;
using Domain.Entities;
using Domain.Errors;
using Domain.Interfaces;
using Domain.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class ProjectService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IJwtTokenService jwtTokenService,
    ILogger<ProjectService> logger) : IProjectService
{
    // get projects
    public async Task<Result<IEnumerable<ProjectResponseDto>>> GetProjectsByUserIdAsync(string accessToken)
    {
        logger.LogInformation("Getting projects for user");
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        var projects = await unitOfWork.ProjectRepository.GetAllByUserIdAsync(userId);

        var projectWithSkills = mapper.Map<IEnumerable<ProjectResponseDto>>(projects);
        logger.LogInformation("Successfully retrieved projects for user {UserId}", userId);
        return Result<IEnumerable<ProjectResponseDto>>.Success(projectWithSkills);
    }

    // add projects
    public async Task<Result<bool>> AddProjectAsync(ProjectRequestDto projectRequestDto, string accessToken)
    {
        logger.LogInformation("Adding new project");
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);

        var project = mapper.Map<Project>(projectRequestDto);
        project.UserId = userId;

        var result = await unitOfWork.ProjectRepository.AddAsync(project);
        if (!result)
        {
            logger.LogError("Failed to add project for user {UserId}", userId);
            return Result<bool>.Failure(ProjectErrors.FailedToAddProject(nameof(userId)));
        }

        await unitOfWork.CommitAsync();
        logger.LogInformation("Successfully added project for user {UserId}", userId);
        return Result<bool>.Success(result);
    }

    // edit projects
    public async Task<Result<bool>> UpdateProjectAsync(ProjectRequestDto experienceRequestDto, Guid projectId,
        string accessToken)
    {
        logger.LogInformation("Updating project {ProjectId}", projectId);
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        var existingProject = await unitOfWork.ProjectRepository.GetByUserIdAsync(userId, projectId);
        if (existingProject is null)
        {
            logger.LogWarning("Project {ProjectId} not found for user {UserId}", projectId, userId);
            return Result<bool>.Failure(ProjectErrors.ProjectNotBelongToUser(nameof(userId)));
        }

        // Map DTO to existing entity to preserve Id
        mapper.Map(experienceRequestDto, existingProject);
        existingProject.UpdatedAt = DateTime.UtcNow;

        var result = await unitOfWork.ProjectRepository.UpdateAsync(existingProject);
        if (!result)
        {
            logger.LogError("Failed to update project {ProjectId} for user {UserId}", projectId, userId);
            return Result<bool>.Failure(ProjectErrors.FailedToUpdateProject(nameof(userId)));
        }

        await unitOfWork.CommitAsync();
        logger.LogInformation("Successfully updated project {ProjectId}", projectId);
        return Result<bool>.Success(result);
    }

    // delete projects
    public async Task<Result<bool>> DeleteProjectAsync(Guid projectId, string accessToken)
    {
        logger.LogInformation("Deleting project {ProjectId}", projectId);
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        var existingProject = await unitOfWork.ProjectRepository.GetByUserIdAsync(userId, projectId);
        if (existingProject is null)
        {
            logger.LogWarning("Project {ProjectId} not found for user {UserId}", projectId, userId);
            return Result<bool>.Failure(ProjectErrors.FailedToDeleteProject(nameof(userId)));
        }

        var result = await unitOfWork.ProjectRepository.DeleteAsync(existingProject);
        if (!result)
        {
            logger.LogError("Failed to delete project {ProjectId} for user {UserId}", projectId, userId);
            return Result<bool>.Failure(ProjectErrors.FailedToDeleteProject(nameof(userId)));
        }

        await unitOfWork.CommitAsync();
        logger.LogInformation("Successfully deleted project {ProjectId}", projectId);
        return Result<bool>.Success(result);
    }
}