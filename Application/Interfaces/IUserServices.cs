using Application.DTOs.User;

namespace Application.Interfaces;

public interface IUserServices
{
    Task<UserResponseDto?> GetProfileByIdAsync(string accessToken);

    Task<bool> UpdateProfileAsync(UserRequestDto userRequestDto, string accessToken);
}