using Application.DTOs.SocialLink;
using Domain.Common.ResultPattern;

namespace Application.Interfaces;

public interface ISocialLinkService
{
    Task<Result<IEnumerable<SocialLinkResponseDto>>> GetSocialLinksByUserIdAsync();
    Task<Result<bool>> AddSocialLinkAsync(SocialLinkRequestDto socialLinkRequestDto, string accessToken);
    Task<Result<bool>> UpdateSocialLinkAsync(SocialLinkRequestDto socialLinkRequestDto, Guid id, string accessToken);
    Task<Result<bool>> DeleteSocialLinkAsync(Guid id, string accessToken);
}