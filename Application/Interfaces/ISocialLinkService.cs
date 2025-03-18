using Application.DTOs.SocialLink;

namespace Application.Interfaces;

public interface ISocialLinkService
{
    Task<IEnumerable<SocialLinkResponseDto>> GetSocialLinksByUserIdAsync(string accessToken);
    Task<bool> AddSocialLinkAsync(SocialLinkRequestDto socialLinkRequestDto, string accessToken);
    Task<bool> UpdateSocialLinkAsync(SocialLinkRequestDto socialLinkRequestDto, string accessToken);
    Task<bool> DeleteSocialLinkAsync(string accessToken);
}