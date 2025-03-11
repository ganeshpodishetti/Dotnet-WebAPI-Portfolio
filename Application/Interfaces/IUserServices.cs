using Application.DTOs;

namespace Application.Interfaces;

public interface IUserServices
{
    Task<UserProfileDto> GetProfileAsync();
    Task<bool> UpdateProfileAsync(string userId, UserProfileDto userProfileDto);
    Task<bool> DeleteProfileAsync(string userId);
    Task<bool> AddProfileAsync(UserProfileDto userProfileDto);
}