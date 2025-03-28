using Application.DTOs.Education;
using Application.Interfaces;
using AutoMapper;
using Domain.Common.ResultPattern;
using Domain.Entities;
using Domain.Errors;
using Domain.Interfaces;
using Domain.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class EducationService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IJwtTokenService jwtTokenService,
    ILogger<EducationService> logger) // Added ILogger
    : IEducationService
{
    // get education
    public async Task<Result<IEnumerable<EducationResponseDto>>> GetEducationsByIdAsync()
    {
        logger.LogInformation("Retrieving all education records");
        var educations = await unitOfWork.EducationRepository.GetAllAsync();

        var educationsList = mapper.Map<IEnumerable<EducationResponseDto>>(educations).ToList();
        logger.LogDebug("Successfully retrieved {Count} education records", educationsList.Count());
        return Result<IEnumerable<EducationResponseDto>>.Success(educationsList);
    }

    // add education
    public async Task<Result<bool>> AddEducationAsync(EducationRequestDto educationDto, string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        logger.LogInformation("Adding new education record for user: {UserId}", userId);

        var education = mapper.Map<Education>(educationDto);
        education.UserId = userId;

        var result = await unitOfWork.EducationRepository.AddAsync(education);
        if (!result)
        {
            logger.LogError("Failed to add education record for user {UserId}", userId);
            return Result<bool>.Failure(EducationErrors.FailedToAddEducation(nameof(userId)));
        }

        await unitOfWork.CommitAsync();
        logger.LogInformation("Successfully added education record {EducationId} for user {UserId}",
            education.Id,
            userId);

        return Result<bool>.Success(result);
    }

    // update education
    public async Task<Result<bool>> UpdateEducationAsync(EducationRequestDto educationDto, Guid id, string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        logger.LogInformation("Updating education record {EducationId} for user {UserId}", id, userId);

        var existingEducation = await unitOfWork.EducationRepository.GetByUserIdAsync(userId, id);
        if (existingEducation is null)
        {
            logger.LogWarning("Education record {EducationId} does not belong to user {UserId}", id, userId);
            return Result<bool>.Failure(EducationErrors.EducationNotBelongToUser(nameof(userId)));
        }

        mapper.Map(educationDto, existingEducation);
        existingEducation.UpdatedAt = DateTime.UtcNow;

        var result = await unitOfWork.EducationRepository.UpdateAsync(existingEducation);
        if (!result)
        {
            logger.LogError("Failed to edit education record for user {UserId}", userId);
            return Result<bool>.Failure(EducationErrors.FailedToUpdateEducation(nameof(userId)));
        }

        await unitOfWork.CommitAsync();
        logger.LogInformation("Successfully updated education record {EducationId} for user {UserId}", id, userId);

        return Result<bool>.Success(result);
    }

    // delete education
    public async Task<Result<bool>> DeleteEducationAsync(Guid id, string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        logger.LogInformation("Attempting to delete education record {EducationId} for user {UserId}", id, userId);

        var existingEducation = await unitOfWork.EducationRepository.GetByUserIdAsync(userId, id);
        if (existingEducation is null)
        {
            logger.LogWarning("Education record {EducationId} not found for user {UserId}", id, userId);
            return Result<bool>.Failure(EducationErrors.EducationNotBelongToUser(nameof(userId)));
        }

        var result = await unitOfWork.EducationRepository.DeleteAsync(existingEducation);
        if (!result)
        {
            logger.LogError("Failed to delete education record {EducationId} for user {UserId}", id, userId);
            return Result<bool>.Failure(EducationErrors.FailedToDeleteEducation(nameof(userId)));
        }

        // Commit the changes to the database
        await unitOfWork.CommitAsync();
        logger.LogInformation("Successfully deleted education record {EducationId} for user {UserId}", id, userId);

        return Result<bool>.Success(result);
    }
}