using Application.DTOs.Experience;
using Application.Interfaces;
using AutoMapper;
using Domain.Common.ResultPattern;
using Domain.Entities;
using Domain.Errors;
using Domain.Interfaces;
using Domain.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class ExperienceService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IJwtTokenService jwtTokenService,
    ILogger<ExperienceService> logger) : IExperienceService
{
    // get experiences
    public async Task<Result<IEnumerable<ExperienceResponseDto>>> GetExperiencesByUserIdAsync(string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        logger.LogInformation("Retrieving experiences for user: {UserId}", userId);

        var experiences = await unitOfWork.ExperienceRepository.GetAllByUserIdAsync(userId);
        var result = mapper.Map<IEnumerable<ExperienceResponseDto>>(experiences).ToList();

        logger.LogDebug("Successfully retrieved {Count} experience records for user {UserId}",
            result.Count(), userId);
        return Result<IEnumerable<ExperienceResponseDto>>.Success(result);
    }

    // add experiences
    public async Task<Result<bool>> AddExperienceAsync(ExperienceRequestDto experienceRequestDto, string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        logger.LogInformation("Adding new experience record for user: {UserId}", userId);

        var experience = mapper.Map<Experience>(experienceRequestDto);
        experience.UserId = userId;

        var result = await unitOfWork.ExperienceRepository.AddAsync(experience);
        if (!result)
        {
            logger.LogError("Failed to add experience record for user {UserId}", userId);
            return Result<bool>.Failure(ExperienceErrors.FailedToAddExperience(userId.ToString()));
        }

        await unitOfWork.CommitAsync();
        logger.LogInformation("Successfully added experience record {ExperienceId} for user {UserId}",
            experience.Id, userId);
        return Result<bool>.Success(result);
    }

    // update experiences
    public async Task<Result<bool>> UpdateExperienceAsync(ExperienceRequestDto experienceRequestDto, Guid experienceId,
        string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        logger.LogInformation("Updating experience record {ExperienceId} for user {UserId}",
            experienceId, userId);

        var existingExperience = await unitOfWork.ExperienceRepository.GetByUserIdAsync(userId, experienceId);
        if (existingExperience is null)
        {
            logger.LogWarning("Experience record {ExperienceId} not found for user {UserId}",
                experienceId, userId);
            return Result<bool>.Failure(ExperienceErrors.ExperienceNotBelongToUser(userId.ToString()));
        }

        mapper.Map(experienceRequestDto, existingExperience);
        existingExperience.UpdatedAt = DateTime.UtcNow;

        var result = await unitOfWork.ExperienceRepository.UpdateAsync(existingExperience);
        if (!result)
        {
            logger.LogError("Failed to update experience record {ExperienceId} for user {UserId}",
                experienceId, userId);
            return Result<bool>.Failure(ExperienceErrors.FailedToUpdateExperience(userId.ToString()));
        }

        await unitOfWork.CommitAsync();
        logger.LogInformation("Successfully updated experience record {ExperienceId} for user {UserId}",
            experienceId, userId);
        return Result<bool>.Success(result);
    }

    // delete experiences
    public async Task<Result<bool>> DeleteExperienceAsync(Guid experienceId, string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        logger.LogInformation("Attempting to delete experience record {ExperienceId} for user {UserId}",
            experienceId, userId);

        var existingExperience = await unitOfWork.ExperienceRepository.GetByUserIdAsync(userId, experienceId);
        if (existingExperience is null)
        {
            logger.LogWarning("Experience record {ExperienceId} not found for user {UserId}",
                experienceId, userId);
            return Result<bool>.Failure(ExperienceErrors.ExperienceNotBelongToUser(userId.ToString()));
        }

        var result = await unitOfWork.ExperienceRepository.DeleteAsync(existingExperience);
        if (!result)
        {
            logger.LogError("Failed to delete experience record {ExperienceId} for user {UserId}",
                experienceId, userId);
            return Result<bool>.Failure(ExperienceErrors.FailedToDeleteExperience(userId.ToString()));
        }

        await unitOfWork.CommitAsync();
        logger.LogInformation("Successfully deleted experience record {ExperienceId} for user {UserId}",
            experienceId, userId);
        return Result<bool>.Success(result);
    }
}