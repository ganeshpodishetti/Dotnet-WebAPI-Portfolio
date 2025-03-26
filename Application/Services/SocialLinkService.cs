using Application.DTOs.SocialLink;
using Application.Interfaces;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.Interfaces;
using Domain.UnitOfWork;

namespace Application.Services;

public class SocialLinkService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IJwtTokenService jwtTokenService) : ISocialLinkService
{
    // get social media links
    public async Task<Result<IEnumerable<SocialLinkResponseDto>>> GetSocialLinksByUserIdAsync(string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);

        var socialLinks = await unitOfWork.SocialLinkRepository.GetAllByUserIdAsync(userId);
        if (socialLinks is null)
            return Result<IEnumerable<SocialLinkResponseDto>>.Failure(new GeneralError("user_doesn't_exists",
                "User does not exist to get social media links.",
                StatusCode.NotFound));
        var response = mapper.Map<IEnumerable<SocialLinkResponseDto>>(socialLinks);
        return Result<IEnumerable<SocialLinkResponseDto>>.Success(response);
    }

    // add social media links
    public async Task<Result<bool>> AddSocialLinkAsync(SocialLinkRequestDto socialLinkRequestDto, string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);

        var existingUser = await unitOfWork.UserRepository.GetByIdAsync(userId);
        if (existingUser is null)
            return Result<bool>.Failure(new GeneralError("user_doesn't_exists",
                "User does not exist to add social media links.",
                StatusCode.NotFound));

        var socialLink = mapper.Map<SocialLink>(socialLinkRequestDto);
        socialLink.UserId = existingUser.Id;

        var result = await unitOfWork.SocialLinkRepository.AddAsync(socialLink);
        await unitOfWork.CommitAsync();
        return Result<bool>.Success(true);
    }

    // update social media links
    public async Task<Result<bool>> UpdateSocialLinkAsync(SocialLinkRequestDto socialLinkRequestDto, Guid socialLinkId,
        string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        var existingSocialLink = await unitOfWork.SocialLinkRepository.GetByUserIdAsync(userId, socialLinkId);
        if (existingSocialLink is null)
            return Result<bool>.Failure(new GeneralError("user_doesn't_exists",
                "User does not exist to update social media links.",
                StatusCode.NotFound));

        // Map DTO to existing entity to preserve id
        mapper.Map(socialLinkRequestDto, existingSocialLink);
        existingSocialLink.UpdatedAt = DateTime.UtcNow;

        var result = await unitOfWork.SocialLinkRepository.UpdateAsync(existingSocialLink);
        await unitOfWork.CommitAsync();
        return Result<bool>.Success(result);
    }

    // delete social media links
    public async Task<Result<bool>> DeleteSocialLinkAsync(Guid socialLinkId, string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        var existingSocialLink = await unitOfWork.SocialLinkRepository.GetByUserIdAsync(userId, socialLinkId);
        if (existingSocialLink is null)
            return Result<bool>.Failure(new GeneralError("user_doesn't_exists",
                "User does not exist to delete social media links.",
                StatusCode.NotFound));

        var result = await unitOfWork.SocialLinkRepository.DeleteAsync(existingSocialLink);
        await unitOfWork.CommitAsync();
        return Result<bool>.Success(result);
    }
}