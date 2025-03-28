using Application.DTOs.SocialLink;
using Application.Interfaces;
using AutoMapper;
using Domain.Common.ResultPattern;
using Domain.Entities;
using Domain.Errors;
using Domain.Interfaces;
using Domain.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class SocialLinkService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IJwtTokenService jwtTokenService,
    ILogger<SocialLinkService> logger) : ISocialLinkService
{
    // get social media links
    public async Task<Result<IEnumerable<SocialLinkResponseDto>>> GetSocialLinksByUserIdAsync()
    {
        logger.LogInformation("Getting social links for user");
        var socialLinks = await unitOfWork.SocialLinkRepository.GetAllAsync();

        var response = mapper.Map<IEnumerable<SocialLinkResponseDto>>(socialLinks);
        logger.LogInformation("Successfully retrieved social links for user");
        return Result<IEnumerable<SocialLinkResponseDto>>.Success(response);
    }

    // add social media links
    public async Task<Result<bool>> AddSocialLinkAsync(SocialLinkRequestDto socialLinkRequestDto, string accessToken)
    {
        logger.LogInformation("Adding new social link for user");
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);

        var socialLink = mapper.Map<SocialLink>(socialLinkRequestDto);
        socialLink.UserId = userId;

        var result = await unitOfWork.SocialLinkRepository.AddAsync(socialLink);
        if (!result)
        {
            logger.LogError("Failed to add social link for user {UserId}", userId);
            return Result<bool>.Failure(SocialLinkErrors.FailedToAddSocialLink(nameof(userId)));
        }

        await unitOfWork.CommitAsync();
        logger.LogInformation("Successfully added social link for user {UserId}", userId);
        return Result<bool>.Success(true);
    }

    // update social media links
    public async Task<Result<bool>> UpdateSocialLinkAsync(SocialLinkRequestDto socialLinkRequestDto, Guid socialLinkId,
        string accessToken)
    {
        logger.LogInformation("Updating social link {SocialLinkId}", socialLinkId);
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        var existingSocialLink = await unitOfWork.SocialLinkRepository.GetByUserIdAsync(userId, socialLinkId);
        if (existingSocialLink is null)
        {
            logger.LogWarning("Social link {SocialLinkId} not found for user {UserId}", socialLinkId, userId);
            return Result<bool>.Failure(SocialLinkErrors.SocialLinkNotBelongToUser(nameof(userId)));
        }

        // Map DTO to existing entity to preserve id
        mapper.Map(socialLinkRequestDto, existingSocialLink);
        existingSocialLink.UpdatedAt = DateTime.UtcNow;

        var result = await unitOfWork.SocialLinkRepository.UpdateAsync(existingSocialLink);
        if (!result)
        {
            logger.LogError("Failed to update social link for user {UserId}", userId);
            return Result<bool>.Failure(SocialLinkErrors.FailedToUpdateSocialLink(nameof(socialLinkId)));
        }

        await unitOfWork.CommitAsync();
        logger.LogInformation("Successfully updated social link {SocialLinkId}", socialLinkId);
        return Result<bool>.Success(result);
    }

    // delete social media links
    public async Task<Result<bool>> DeleteSocialLinkAsync(Guid socialLinkId, string accessToken)
    {
        logger.LogInformation("Deleting social link {SocialLinkId}", socialLinkId);
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        var existingSocialLink = await unitOfWork.SocialLinkRepository.GetByUserIdAsync(userId, socialLinkId);
        if (existingSocialLink is null)
        {
            logger.LogWarning("Social link {SocialLinkId} not found for user {UserId}", socialLinkId, userId);
            return Result<bool>.Failure(SocialLinkErrors.SocialLinkNotBelongToUser(nameof(userId)));
        }

        var result = await unitOfWork.SocialLinkRepository.DeleteAsync(existingSocialLink);
        if (!result)
        {
            logger.LogError("Failed to delete social link for user {UserId}", userId);
            return Result<bool>.Failure(SocialLinkErrors.FailedToDeleteSocialLink(nameof(socialLinkId)));
        }

        await unitOfWork.CommitAsync();
        logger.LogInformation("Successfully deleted social link {SocialLinkId}", socialLinkId);
        return Result<bool>.Success(result);
    }
}