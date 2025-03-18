using Application.DTOs.User;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.UnitOfWork;

namespace Application.Services;

public class UserServices(IUnitOfWork unitOfWork, IMapper mapper, IJwtTokenService jwtTokenService)
    : IUserServices
{
    // Get user profile by ID
    public async Task<UserResponseDto?> GetProfileByIdAsync(string accessToken)
    {
        var userIdString = jwtTokenService.GetUserIdFromToken(accessToken);
        if (!Guid.TryParse(userIdString, out var userId))
            throw new InvalidOperationException("Invalid user ID format");

        var user = await unitOfWork.UserRepository.GetUserWithDetailsAsync(userId);
        if (user is null)
            throw new NotFoundException(nameof(user), userId.ToString());

        var result = mapper.Map<UserResponseDto>(user);
        return result;
    }

    // Update user profile
    public async Task<bool> UpdateProfileAsync(UserRequestDto userRequestDto, string accessToken)
    {
        var userIdString = jwtTokenService.GetUserIdFromToken(accessToken);
        if (!Guid.TryParse(userIdString, out var userId))
            throw new InvalidOperationException("Invalid user ID format");

        var toUpdate = await unitOfWork.UserRepository.GetByIdAsync(userId);
        if (toUpdate is null)
            throw new NotFoundException(nameof(toUpdate), userId.ToString());

        // Update existing entity instead of creating new
        return await UpdateExistingUserProfile(toUpdate, userRequestDto);
    }

    // Add new profile or update existing one
    public async Task<bool> AddProfileAsync(UserRequestDto userRequestDto, string accessToken)
    {
        var userIdString = jwtTokenService.GetUserIdFromToken(accessToken);
        if (!Guid.TryParse(userIdString, out var userId))
            throw new InvalidOperationException("Invalid user ID format");

        var toAdd = await unitOfWork.UserRepository.GetByIdAsync(userId);
        if (toAdd is null)
            throw new NotFoundException(nameof(toAdd), userId.ToString());

        return await UpdateExistingUserProfile(toAdd, userRequestDto);
    }

    // Update existing user profile
    private async Task<bool> UpdateExistingUserProfile(User user, UserRequestDto userRequestDto)
    {
        user.UpdatedAt = DateTime.UtcNow;

        mapper.Map(userRequestDto, user.AboutMe);
        var result = await unitOfWork.UserRepository.UpdateAsync(user);
        await unitOfWork.CommitAsync();
        return result;
    }
}