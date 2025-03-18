using Application.DTOs.Skill;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.UnitOfWork;

namespace Application.Services;

public class SkillService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IJwtTokenService jwtTokenService) : ISkillService
{
    public async Task<List<SkillResponseDto>> GetAllSkillsByUserIdAsync(string accessToken)
    {
        var userIdString = jwtTokenService.GetUserIdFromToken(accessToken);
        if (!Guid.TryParse(userIdString, out var userId))
            throw new InvalidOperationException("Invalid user ID format");

        var skills = await unitOfWork.SkillRepository.GetAllByUserIdAsync(userId);
        var result = mapper.Map<List<SkillResponseDto>>(skills);
        return result;
    }

    public async Task<bool> AddSkillAsync(SkillRequestDto skillRequestDto, string accessToken)
    {
        var userIdString = jwtTokenService.GetUserIdFromToken(accessToken);
        if (!Guid.TryParse(userIdString, out var userId))
            throw new InvalidOperationException("Invalid user ID format");

        var existingUser = await unitOfWork.UserRepository.GetByIdAsync(userId);
        if (existingUser is null)
            throw new Exception("User does not exist to add experience");

        var toAddSkill = mapper.Map<Skill>(skillRequestDto);
        toAddSkill.UserId = existingUser.Id;
        toAddSkill.CreatedAt = DateTime.UtcNow;
        toAddSkill.UpdatedAt = DateTime.UtcNow;

        var result = await unitOfWork.SkillRepository.AddAsync(toAddSkill);
        await unitOfWork.CommitAsync();
        return result;
    }

    public async Task<bool> UpdateSkillAsync(SkillRequestDto skillRequestDto, string accessToken)
    {
        var existingSkill = await GetExperienceByUserId(accessToken);
        if (existingSkill is null)
            throw new Exception("User does not exist to update experience.");

        // Map DTO to existing entity to preserve Id
        mapper.Map(skillRequestDto, existingSkill);
        existingSkill.UpdatedAt = DateTime.UtcNow;

        var result = await unitOfWork.SkillRepository.UpdateAsync(existingSkill);
        await unitOfWork.CommitAsync();
        return result;
    }

    public async Task<bool> DeleteSkillAsync(string accessToken)
    {
        var existingSkill = await GetExperienceByUserId(accessToken);
        if (existingSkill is null)
            throw new Exception("User does not exist to delete experience.");

        var result = await unitOfWork.SkillRepository.DeleteAsync(existingSkill);
        await unitOfWork.CommitAsync();
        return result;
    }

    private async Task<Skill?> GetExperienceByUserId(string accessToken)
    {
        var userIdString = jwtTokenService.GetUserIdFromToken(accessToken);
        if (!Guid.TryParse(userIdString, out var userId))
            throw new InvalidOperationException("Invalid user ID format");

        return await unitOfWork.SkillRepository.GetByUserIdAsync(userId);
    }
}