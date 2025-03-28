using Application.DTOs.Skill;
using Application.Interfaces;
using AutoMapper;
using Domain.Common.ResultPattern;
using Domain.Entities;
using Domain.Errors;
using Domain.Interfaces;
using Domain.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class SkillService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IJwtTokenService jwtTokenService,
    ILogger<SkillService> logger) : ISkillService
{
    // get skills
    public async Task<Result<IEnumerable<SkillResponseDto>>> GetAllSkillsByUserIdAsync()
    {
        logger.LogInformation("Getting all skills for user");
        var skills = await unitOfWork.SkillRepository.GetAllAsync();

        var result = mapper.Map<IEnumerable<SkillResponseDto>>(skills);
        logger.LogInformation("Successfully retrieved skills for user");
        return Result<IEnumerable<SkillResponseDto>>.Success(result);
    }

    // add skills
    public async Task<Result<bool>> AddSkillAsync(SkillRequestDto skillRequestDto, string accessToken)
    {
        logger.LogInformation("Adding new skill for user");
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);

        var toAddSkill = mapper.Map<Skill>(skillRequestDto);
        toAddSkill.UserId = userId;

        var result = await unitOfWork.SkillRepository.AddAsync(toAddSkill);
        if (!result)
        {
            logger.LogError("Failed to add skill for user {UserId}", userId);
            return Result<bool>.Failure(SkillErrors.FailedToAddSkill(nameof(userId)));
        }

        await unitOfWork.CommitAsync();
        logger.LogInformation("Successfully added skill for user {UserId}", userId);
        return Result<bool>.Success(result);
    }

    // update skills
    public async Task<Result<bool>> UpdateSkillAsync(SkillRequestDto skillRequestDto, Guid skillId, string accessToken)
    {
        logger.LogInformation("Updating skill {SkillId}", skillId);
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        var existingSkill = await unitOfWork.SkillRepository.GetByUserIdAsync(userId, skillId);
        if (existingSkill is null)
        {
            logger.LogWarning("Skill {SkillId} not found for user {UserId}", skillId, userId);
            return Result<bool>.Failure(SkillErrors.SkillNotBelongToUser(nameof(userId)));
        }

        // Map DTO to existing entity to preserve Id
        mapper.Map(skillRequestDto, existingSkill);
        existingSkill.UpdatedAt = DateTime.UtcNow;

        var result = await unitOfWork.SkillRepository.UpdateAsync(existingSkill);
        if (!result)
        {
            logger.LogError("Failed to update skill for user {UserId}", userId);
            return Result<bool>.Failure(SkillErrors.FailedToUpdateSkill(nameof(skillId)));
        }

        await unitOfWork.CommitAsync();
        logger.LogInformation("Successfully updated skill {SkillId}", skillId);
        return Result<bool>.Success(result);
    }

    // delete skills
    public async Task<Result<bool>> DeleteSkillAsync(Guid skillId, string accessToken)
    {
        logger.LogInformation("Deleting skill {SkillId}", skillId);
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        var existingSkill = await unitOfWork.SkillRepository.GetByUserIdAsync(userId, skillId);
        if (existingSkill is null)
        {
            logger.LogWarning("Skill {SkillId} not found for user {UserId}", skillId, userId);
            return Result<bool>.Failure(SkillErrors.SkillNotBelongToUser(nameof(userId)));
        }

        var result = await unitOfWork.SkillRepository.DeleteAsync(existingSkill);
        if (!result)
        {
            logger.LogError("Failed to delete skill for user {UserId}", userId);
            return Result<bool>.Failure(SkillErrors.FailedToDeleteSkill(nameof(skillId)));
        }

        await unitOfWork.CommitAsync();
        logger.LogInformation("Successfully deleted skill {SkillId}", skillId);
        return Result<bool>.Success(result);
    }
}