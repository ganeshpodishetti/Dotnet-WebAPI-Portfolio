using Application.DTOs.Experience;
using Application.Interfaces;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.Interfaces;
using Domain.UnitOfWork;

namespace Application.Services;

public class ExperienceService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IJwtTokenService jwtTokenService) : IExperienceService
{
    // get experiences
    public async Task<Result<IEnumerable<ExperienceResponseDto>>> GetExperiencesByUserIdAsync(string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);

        var experiences = await unitOfWork.ExperienceRepository.GetAllByUserIdAsync(userId);
        if (experiences is null)
            return Result<IEnumerable<ExperienceResponseDto>>.Failure(new GeneralError("user_doesn't_exists",
                "User does not exist to get experience", StatusCode.NotFound));
        var result = mapper.Map<IEnumerable<ExperienceResponseDto>>(experiences);
        return Result<IEnumerable<ExperienceResponseDto>>.Success(result);
    }

    // add experiences
    public async Task<Result<bool>> AddExperienceAsync(ExperienceRequestDto experienceRequestDto, string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);

        var existingUser = await unitOfWork.UserRepository.GetByIdAsync(userId);
        if (existingUser is null)
            return Result<bool>.Failure(new GeneralError("user_doesn't_exists", "User does not exist to add experience",
                StatusCode.NotFound));

        var experience = mapper.Map<Experience>(experienceRequestDto);
        experience.UserId = existingUser.Id;

        var result = await unitOfWork.ExperienceRepository.AddAsync(experience);
        await unitOfWork.CommitAsync();
        return Result<bool>.Success(result);
    }

    // update experiences
    public async Task<Result<bool>> UpdateExperienceAsync(ExperienceRequestDto experienceRequestDto, Guid experienceId,
        string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);

        var existingExperience = await unitOfWork.ExperienceRepository.GetByUserIdAsync(userId, experienceId);
        if (existingExperience is null)
            //throw new Exception("User does not exist to update experience.");
            return Result<bool>.Failure(new GeneralError("user_doesn't_exists",
                "User does not exist to update experience",
                StatusCode.NotFound));

        // Map DTO to existing entity to preserve Id
        mapper.Map(experienceRequestDto, existingExperience);
        existingExperience.UpdatedAt = DateTime.UtcNow;

        var result = await unitOfWork.ExperienceRepository.UpdateAsync(existingExperience);
        await unitOfWork.CommitAsync();
        return Result<bool>.Success(result);
    }

    // delete experiences
    public async Task<Result<bool>> DeleteExperienceAsync(Guid experienceId, string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);

        var existingExperience = await unitOfWork.ExperienceRepository.GetByUserIdAsync(userId, experienceId);
        if (existingExperience is null)
            //throw new Exception("User does not exist to delete experience.");
            return Result<bool>.Failure(new GeneralError("user_doesn't_exists",
                "User does not exist to delete experience",
                StatusCode.NotFound));

        var result = await unitOfWork.ExperienceRepository.DeleteAsync(existingExperience);
        await unitOfWork.CommitAsync();
        return Result<bool>.Success(result);
    }
}