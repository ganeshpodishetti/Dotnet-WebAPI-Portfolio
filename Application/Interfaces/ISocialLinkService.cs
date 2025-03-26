using Application.DTOs.SocialLink;
using Domain.Common;

namespace Application.Interfaces;

public interface ISocialLinkService
{
    Task<Result<IEnumerable<SocialLinkResponseDto>>> GetSocialLinksByUserIdAsync(string accessToken);
    Task<Result<bool>> AddSocialLinkAsync(SocialLinkRequestDto socialLinkRequestDto, string accessToken);
    Task<Result<bool>> UpdateSocialLinkAsync(SocialLinkRequestDto socialLinkRequestDto, Guid id, string accessToken);
    Task<Result<bool>> DeleteSocialLinkAsync(Guid id, string accessToken);
}