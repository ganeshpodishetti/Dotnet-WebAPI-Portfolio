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
    // get social media links
    public async Task<IEnumerable<SocialLinkResponseDto>> GetSocialLinksByUserIdAsync(string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);

        var socialLinks = await unitOfWork.SocialLinkRepository.GetAllByUserIdAsync(userId);
        var response = mapper.Map<IEnumerable<SocialLinkResponseDto>>(socialLinks);
        return response;
    }

    // add social media links
    public async Task<bool> AddSocialLinkAsync(SocialLinkRequestDto socialLinkRequestDto, string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);

        var existingUser = await unitOfWork.UserRepository.GetByIdAsync(userId);
        if (existingUser is null)
            throw new Exception("User does not exist to add experience");

        var socialLink = mapper.Map<SocialLink>(socialLinkRequestDto);
        socialLink.UserId = existingUser.Id;

        var result = await unitOfWork.SocialLinkRepository.AddAsync(socialLink);
        await unitOfWork.CommitAsync();
        return result;
    }

    // update social media links
    public async Task<bool> UpdateSocialLinkAsync(SocialLinkRequestDto socialLinkRequestDto, Guid socialLinkId,
        string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        var existingSocialLink = await unitOfWork.SocialLinkRepository.GetByUserIdAsync(userId, socialLinkId);
        if (existingSocialLink is null)
            throw new Exception("User does not exist to update experience.");

        // Map DTO to existing entity to preserve id
        mapper.Map(socialLinkRequestDto, existingSocialLink);
        existingSocialLink.UpdatedAt = DateTime.UtcNow;

        var result = await unitOfWork.SocialLinkRepository.UpdateAsync(existingSocialLink);
        await unitOfWork.CommitAsync();
        return result;
    }

    // delete social media links
    public async Task<bool> DeleteSocialLinkAsync(Guid socialLinkId, string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        var existingSocialLink = await unitOfWork.SocialLinkRepository.GetByUserIdAsync(userId, socialLinkId);
        if (existingSocialLink is null)
            throw new Exception("User does not exist to delete experience.");

        var result = await unitOfWork.SocialLinkRepository.DeleteAsync(existingSocialLink);
        await unitOfWork.CommitAsync();
        return result;
    }
}