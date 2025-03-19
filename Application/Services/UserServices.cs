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
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);

        var user = await unitOfWork.UserRepository.GetUserWithDetailsAsync(userId)
                   ?? throw new NotFoundException(nameof(User), userId.ToString());

        return mapper.Map<UserResponseDto>(user);
    }

    // Update user profile
    public async Task<bool> UpdateProfileAsync(UserRequestDto userRequestDto, string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);

        var toUpdate = await unitOfWork.UserRepository.GetByIdAsync(userId);
        if (toUpdate is null)
            throw new NotFoundException(nameof(toUpdate), userId.ToString());

        toUpdate.UpdatedAt = DateTime.UtcNow;

        mapper.Map(userRequestDto, toUpdate.AboutMe);
        var result = await unitOfWork.UserRepository.UpdateAsync(toUpdate);
        await unitOfWork.CommitAsync();
        return result;
    }
}