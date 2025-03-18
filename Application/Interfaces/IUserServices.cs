using Application.DTOs.User;

namespace Application.Interfaces;

public interface IUserServices
{
    Task<UserResponseDto?> GetProfileByIdAsync(string accessToken);

    Task<bool> UpdateProfileAsync(UserRequestDto userRequestDto, string accessToken);

    //Task<bool> DeleteProfileAsync(string accessToken);
    Task<bool> AddProfileAsync(UserRequestDto userRequestDto, string accessToken);
}