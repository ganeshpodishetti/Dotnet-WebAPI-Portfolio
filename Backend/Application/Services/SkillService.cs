using Application.DTOs.Skill;
using Application.Interfaces;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.Interfaces;
using Domain.UnitOfWork;

namespace Application.Services;

public class SkillService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IJwtTokenService jwtTokenService) : ISkillService
{
    // get skills
    public async Task<Result<IEnumerable<SkillResponseDto>>> GetAllSkillsByUserIdAsync(string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        var skills = await unitOfWork.SkillRepository.GetAllByUserIdAsync(userId);

        var result = mapper.Map<IEnumerable<SkillResponseDto>>(skills);
        return Result<IEnumerable<SkillResponseDto>>.Success(result);
    }

    // add skills
    public async Task<Result<bool>> AddSkillAsync(SkillRequestDto skillRequestDto, string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);

        var toAddSkill = mapper.Map<Skill>(skillRequestDto);
        toAddSkill.UserId = userId;

        var result = await unitOfWork.SkillRepository.AddAsync(toAddSkill);
        await unitOfWork.CommitAsync();
        return Result<bool>.Success(result);
    }

    // update skills
    public async Task<Result<bool>> UpdateSkillAsync(SkillRequestDto skillRequestDto, Guid skillId, string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        var existingSkill = await unitOfWork.SkillRepository.GetByUserIdAsync(userId, skillId);
        if (existingSkill is null)
            return Result<bool>.Failure(new GeneralError("user_doesn't_exists",
                "User does not exist to update skills.",
                StatusCode.NotFound));

        // Map DTO to existing entity to preserve Id
        mapper.Map(skillRequestDto, existingSkill);
        existingSkill.UpdatedAt = DateTime.UtcNow;

        var result = await unitOfWork.SkillRepository.UpdateAsync(existingSkill);
        await unitOfWork.CommitAsync();
        return Result<bool>.Success(result);
    }

    // delete skills
    public async Task<Result<bool>> DeleteSkillAsync(Guid skillId, string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        var existingSkill = await unitOfWork.SkillRepository.GetByUserIdAsync(userId, skillId);
        if (existingSkill is null)
            return Result<bool>.Failure(new GeneralError("user_doesn't_exists",
                "User does not exist to delete skills.",
                StatusCode.NotFound));

        var result = await unitOfWork.SkillRepository.DeleteAsync(existingSkill);
        await unitOfWork.CommitAsync();
        return Result<bool>.Success(result);
    }
}