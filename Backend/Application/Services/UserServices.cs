using Application.DTOs.User;
using Application.Interfaces;
using AutoMapper;
using Domain.Common.ResultPattern;
using Domain.Errors;
using Domain.Interfaces;
using Domain.UnitOfWork;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class UserServices(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IJwtTokenService jwtTokenService,
    ILogger<UserServices> logger) : IUserServices
{
    // Get user profile by ID
    public async Task<Result<UserResponseDto?>> GetProfileByIdAsync(string accessToken)
    {
        logger.LogInformation("Getting user profile");
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        var user = await unitOfWork.UserRepository.GetUserWithDetailsAsync(userId);

        if (user == null)
        {
            logger.LogWarning("User profile not found for ID {UserId}", userId);
            return Result<UserResponseDto?>.Success(null);
        }

        var responseDto = mapper.Map<UserResponseDto>(user);
        logger.LogInformation("Successfully retrieved profile for user {UserId}", userId);
        return Result<UserResponseDto?>.Success(responseDto);
    }

    // Update user profile
    public async Task<Result<bool>> UpdateProfileAsync(UserRequestDto userRequestDto, string accessToken)
    {
        logger.LogInformation("Updating user profile");
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);

        var toUpdate = await unitOfWork.UserRepository.GetByIdAsync(userId);
        if (toUpdate is null)
        {
            logger.LogWarning("User not found for ID {UserId}", userId);
            return Result.Failure<bool>(UserErrors.UserNotFound(nameof(userId)));
        }

        toUpdate.UpdatedAt = DateTime.UtcNow;
        mapper.Map(userRequestDto, toUpdate.AboutMe);

        var result = await unitOfWork.UserRepository.UpdateAsync(toUpdate);
        if (!result)
        {
            logger.LogError("Failed to update profile for user {UserId}", userId);
            return Result.Failure<bool>(UserErrors.FailedToUpdateUser(nameof(userId)));
        }

        await unitOfWork.CommitAsync();
        logger.LogInformation("Successfully updated profile for user {UserId}", userId);
        return Result<bool>.Success(true);
    }
}