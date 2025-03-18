using Application.DTOs.Experience;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.UnitOfWork;

namespace Application.Services;

public class ExperienceService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IJwtTokenService jwtTokenService) : IExperienceService
{
    public async Task<List<ExperienceResponseDto>> GetExperiencesByUserIdAsync(string accessToken)
    {
        var userIdString = jwtTokenService.GetUserIdFromToken(accessToken);
        if (!Guid.TryParse(userIdString, out var userId))
            throw new InvalidOperationException("Invalid user ID format");

        var experiences = await unitOfWork.ExperienceRepository.GetAllByUserIdAsync(userId);
        var result = mapper.Map<List<ExperienceResponseDto>>(experiences);
        return result;
    }

    public async Task<bool> AddExperienceAsync(ExperienceRequestDto experienceRequestDto, string accessToken)
    {
        var userIdString = jwtTokenService.GetUserIdFromToken(accessToken);
        if (!Guid.TryParse(userIdString, out var userId))
            throw new InvalidOperationException("Invalid user ID format");

        var existingUser = await unitOfWork.UserRepository.GetByIdAsync(userId);
        if (existingUser is null)
            throw new Exception("User does not exist to add experience");

        var experience = mapper.Map<Experience>(experienceRequestDto);
        experience.UserId = existingUser.Id;
        experience.CreatedAt = DateTime.UtcNow;
        experience.UpdatedAt = DateTime.UtcNow;

        var result = await unitOfWork.ExperienceRepository.AddAsync(experience);
        await unitOfWork.CommitAsync();
        return result;
    }

    public async Task<bool> UpdateExperienceAsync(ExperienceRequestDto experienceRequestDto, string accessToken)
    {
        var existingExperience = await GetExperienceByUserId(accessToken);
        if (existingExperience is null)
            throw new Exception("User does not exist to update experience.");

        // Map DTO to existing entity to preserve Id
        mapper.Map(experienceRequestDto, existingExperience);
        existingExperience.UpdatedAt = DateTime.UtcNow;

        var result = await unitOfWork.ExperienceRepository.UpdateAsync(existingExperience);
        await unitOfWork.CommitAsync();
        return result;
    }

    public async Task<bool> DeleteExperienceAsync(string accessToken)
    {
        var existingExperience = await GetExperienceByUserId(accessToken);
        if (existingExperience is null)
            throw new Exception("User does not exist to delete experience.");

        var result = await unitOfWork.ExperienceRepository.DeleteAsync(existingExperience);
        await unitOfWork.CommitAsync();
        return result;
    }

    private async Task<Experience?> GetExperienceByUserId(string accessToken)
    {
        var userIdString = jwtTokenService.GetUserIdFromToken(accessToken);
        if (!Guid.TryParse(userIdString, out var userId))
            throw new InvalidOperationException("Invalid user ID format");

        return await unitOfWork.ExperienceRepository.GetByUserIdAsync(userId);
    }
}