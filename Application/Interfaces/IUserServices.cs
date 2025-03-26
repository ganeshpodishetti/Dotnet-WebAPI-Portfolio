using Application.DTOs.User;
using Domain.Common;

namespace Application.Interfaces;

public interface IUserServices
{
    Task<Result<UserResponseDto?>> GetProfileByIdAsync(string accessToken);

    Task<Result<bool>> UpdateProfileAsync(UserRequestDto userRequestDto, string accessToken);
}