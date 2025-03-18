using Application.DTOs.SocialLink;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.UnitOfWork;

namespace Application.Services;

public class SocialLinkService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IJwtTokenService jwtTokenService) : ISocialLinkService
{
    public async Task<IEnumerable<SocialLinkResponseDto>> GetSocialLinksByUserIdAsync(string accessToken)
    {
        var userIdString = jwtTokenService.GetUserIdFromToken(accessToken);
        if (!Guid.TryParse(userIdString, out var userId))
            throw new InvalidOperationException("Invalid user ID format");

        var socialLinks = await unitOfWork.SocialLinkRepository.GetAllByUserIdAsync(userId);
        var response = mapper.Map<IEnumerable<SocialLinkResponseDto>>(socialLinks);
        return response;
    }

    public async Task<bool> AddSocialLinkAsync(SocialLinkRequestDto socialLinkRequestDto, string accessToken)
    {
        var userIdString = jwtTokenService.GetUserIdFromToken(accessToken);
        if (!Guid.TryParse(userIdString, out var userId))
            throw new InvalidOperationException("Invalid user ID format");

        var existingUser = await unitOfWork.UserRepository.GetByIdAsync(userId);
        if (existingUser is null)
            throw new Exception("User does not exist to add experience");

        var socialLink = mapper.Map<SocialLink>(socialLinkRequestDto);
        socialLink.UserId = existingUser.Id;
        socialLink.CreatedAt = DateTime.UtcNow;
        socialLink.UpdatedAt = DateTime.UtcNow;

        var result = await unitOfWork.SocialLinkRepository.AddAsync(socialLink);
        await unitOfWork.CommitAsync();
        return result;
    }

    public async Task<bool> UpdateSocialLinkAsync(SocialLinkRequestDto socialLinkRequestDto, string accessToken)
    {
        var existingSocialLink = await GetSocialLinkByUserId(accessToken);
        if (existingSocialLink is null)
            throw new Exception("User does not exist to update experience.");

        // Map DTO to existing entity to preserve Id
        mapper.Map(socialLinkRequestDto, existingSocialLink);
        existingSocialLink.UpdatedAt = DateTime.UtcNow;

        var result = await unitOfWork.SocialLinkRepository.UpdateAsync(existingSocialLink);
        await unitOfWork.CommitAsync();
        return result;
    }

    public async Task<bool> DeleteSocialLinkAsync(string accessToken)
    {
        var existingSocialLink = await GetSocialLinkByUserId(accessToken);
        if (existingSocialLink is null)
            throw new Exception("User does not exist to delete experience.");

        var result = await unitOfWork.SocialLinkRepository.DeleteAsync(existingSocialLink);
        await unitOfWork.CommitAsync();
        return result;
    }

    private async Task<SocialLink?> GetSocialLinkByUserId(string accessToken)
    {
        var userIdString = jwtTokenService.GetUserIdFromToken(accessToken);
        if (!Guid.TryParse(userIdString, out var userId))
            throw new InvalidOperationException("Invalid user ID format");

        return await unitOfWork.SocialLinkRepository.GetByUserIdAsync(userId);
    }
}