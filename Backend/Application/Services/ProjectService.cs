using Application.DTOs.Project;
using Application.Interfaces;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.Interfaces;
using Domain.UnitOfWork;

namespace Application.Services;

public class ProjectService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IJwtTokenService jwtTokenService) : IProjectService
{
    // get projects
    public async Task<Result<IEnumerable<ProjectResponseDto>>> GetProjectsByUserIdAsync(string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        var projects = await unitOfWork.ProjectRepository.GetAllByUserIdAsync(userId);

        var projectWithSkills = mapper.Map<IEnumerable<ProjectResponseDto>>(projects);
        return Result<IEnumerable<ProjectResponseDto>>.Success(projectWithSkills);
    }

    // add projects
    public async Task<Result<bool>> AddProjectAsync(ProjectRequestDto projectRequestDto, string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);

        var project = mapper.Map<Project>(projectRequestDto);
        project.UserId = userId;

        var result = await unitOfWork.ProjectRepository.AddAsync(project);
        await unitOfWork.CommitAsync();
        return Result<bool>.Success(result);
    }

    // edit projects
    public async Task<Result<bool>> UpdateProjectAsync(ProjectRequestDto experienceRequestDto, Guid projectId,
        string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        var existingProject = await unitOfWork.ProjectRepository.GetByUserIdAsync(userId, projectId);
        if (existingProject is null)
            return Result<bool>.Failure(new GeneralError("user_doesn't_exists",
                "User does not exist to update experience",
                StatusCode.NotFound));

        // Map DTO to existing entity to preserve Id
        mapper.Map(experienceRequestDto, existingProject);
        existingProject.UpdatedAt = DateTime.UtcNow;

        var result = await unitOfWork.ProjectRepository.UpdateAsync(existingProject);
        await unitOfWork.CommitAsync();
        return Result<bool>.Success(result);
    }

    // delete projects
    public async Task<Result<bool>> DeleteProjectAsync(Guid projectId, string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        var existingProject = await unitOfWork.ProjectRepository.GetByUserIdAsync(userId, projectId);
        if (existingProject is null)
            return Result<bool>.Failure(new GeneralError("user_doesn't_exists",
                "User does not exist to delete experience",
                StatusCode.NotFound));

        var result = await unitOfWork.ProjectRepository.DeleteAsync(existingProject);
        await unitOfWork.CommitAsync();
        return Result<bool>.Success(result);
    }
}