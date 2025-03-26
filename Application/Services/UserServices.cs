using Application.DTOs.User;
using Application.Interfaces;
using AutoMapper;
using Domain.Common;
using Domain.Enums;
using Domain.Errors;
using Domain.Interfaces;
using Domain.UnitOfWork;

namespace Application.Services;

public class UserServices(IUnitOfWork unitOfWork, IMapper mapper, IJwtTokenService jwtTokenService)
    : IUserServices
{
    // Get user profile by ID
    public async Task<Result<UserResponseDto?>> GetProfileByIdAsync(string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);

        var user = await unitOfWork.UserRepository.GetUserWithDetailsAsync(userId);
        if (user is null)
            return Result.Failure<UserResponseDto?>(new GeneralError("user_doesn't_exists",
                "User does not exist", StatusCode.NotFound));

        var responseDto = mapper.Map<UserResponseDto>(user);
        return Result<UserResponseDto?>.Success(responseDto);
    }

    // Update user profile
    public async Task<Result<bool>> UpdateProfileAsync(UserRequestDto userRequestDto, string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);

        var toUpdate = await unitOfWork.UserRepository.GetByIdAsync(userId);
        if (toUpdate is null)
            return Result.Failure<bool>(new GeneralError("user_doesn't_exists",
                "User does not exist to update profile", StatusCode.NotFound));

        toUpdate.UpdatedAt = DateTime.UtcNow;
        mapper.Map(userRequestDto, toUpdate.AboutMe);

        var result = await unitOfWork.UserRepository.UpdateAsync(toUpdate);
        await unitOfWork.CommitAsync();
        return Result<bool>.Success(true);
    }
}