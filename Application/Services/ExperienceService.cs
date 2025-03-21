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
    // get experiences
    public async Task<List<ExperienceResponseDto>> GetExperiencesByUserIdAsync(string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);

        var experiences = await unitOfWork.ExperienceRepository.GetAllByUserIdAsync(userId);
        var result = mapper.Map<List<ExperienceResponseDto>>(experiences);
        return result;
    }

    // add experiences
    public async Task<bool> AddExperienceAsync(ExperienceRequestDto experienceRequestDto, string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);

        var existingUser = await unitOfWork.UserRepository.GetByIdAsync(userId);
        if (existingUser is null)
            throw new Exception("User does not exist to add experience");

        var experience = mapper.Map<Experience>(experienceRequestDto);
        experience.UserId = existingUser.Id;

        var result = await unitOfWork.ExperienceRepository.AddAsync(experience);
        await unitOfWork.CommitAsync();
        return result;
    }

    // update experiences
    public async Task<bool> UpdateExperienceAsync(ExperienceRequestDto experienceRequestDto, Guid experienceId,
        string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);

        var existingExperience = await unitOfWork.ExperienceRepository.GetByUserIdAsync(userId, experienceId);
        if (existingExperience is null)
            throw new Exception("User does not exist to update experience.");

        // Map DTO to existing entity to preserve Id
        mapper.Map(experienceRequestDto, existingExperience);
        existingExperience.UpdatedAt = DateTime.UtcNow;

        var result = await unitOfWork.ExperienceRepository.UpdateAsync(existingExperience);
        await unitOfWork.CommitAsync();
        return result;
    }

    // delete experiences
    public async Task<bool> DeleteExperienceAsync(Guid experienceId, string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);

        var existingExperience = await unitOfWork.ExperienceRepository.GetByUserIdAsync(userId, experienceId);
        if (existingExperience is null)
            throw new Exception("User does not exist to delete experience.");

        var result = await unitOfWork.ExperienceRepository.DeleteAsync(existingExperience);
        await unitOfWork.CommitAsync();
        return result;
    }
}