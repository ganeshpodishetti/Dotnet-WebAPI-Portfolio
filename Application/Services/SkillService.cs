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
    // get skills
    public async Task<List<SkillResponseDto>> GetAllSkillsByUserIdAsync(string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);

        var skills = await unitOfWork.SkillRepository.GetAllByUserIdAsync(userId);
        var result = mapper.Map<List<SkillResponseDto>>(skills);
        return result;
    }

    // add skills
    public async Task<bool> AddSkillAsync(SkillRequestDto skillRequestDto, string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);

        var existingUser = await unitOfWork.UserRepository.GetByIdAsync(userId);
        if (existingUser is null)
            throw new Exception("User does not exist to add experience");

        var toAddSkill = mapper.Map<Skill>(skillRequestDto);
        toAddSkill.UserId = existingUser.Id;

        var result = await unitOfWork.SkillRepository.AddAsync(toAddSkill);
        await unitOfWork.CommitAsync();
        return result;
    }

    // update skills
    public async Task<bool> UpdateSkillAsync(SkillRequestDto skillRequestDto, Guid skillId, string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        var existingSkill = await unitOfWork.SkillRepository.GetByUserIdAsync(userId, skillId);
        if (existingSkill is null)
            throw new Exception("User does not exist to update experience.");

        // Map DTO to existing entity to preserve Id
        mapper.Map(skillRequestDto, existingSkill);
        existingSkill.UpdatedAt = DateTime.UtcNow;

        var result = await unitOfWork.SkillRepository.UpdateAsync(existingSkill);
        await unitOfWork.CommitAsync();
        return result;
    }

    // delete skills
    public async Task<bool> DeleteSkillAsync(Guid skillId, string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        var existingSkill = await unitOfWork.SkillRepository.GetByUserIdAsync(userId, skillId);
        if (existingSkill is null)
            throw new Exception("User does not exist to delete experience.");

        var result = await unitOfWork.SkillRepository.DeleteAsync(existingSkill);
        await unitOfWork.CommitAsync();
        return result;
    }
}