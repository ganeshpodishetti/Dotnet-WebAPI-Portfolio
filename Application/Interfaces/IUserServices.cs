using Application.DTOs.User;

namespace Application.Interfaces;

public interface IUserServices
{
    Task<UserProfileDto?> GetProfileByIdAsync(string accessToken);

    Task<bool> UpdateProfileAsync(UserProfileDto userProfileDto, string accessToken);

    //Task<bool> DeleteProfileAsync(string accessToken);
    Task<bool> AddProfileAsync(UserProfileDto userProfileDto, string accessToken);
}