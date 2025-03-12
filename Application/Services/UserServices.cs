using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.UnitOfWork;

namespace Application.Services;

public class UserServices(IUnitOfWork unitOfWork, IMapper mapper)
    : IUserServices
{
    public async Task<UserProfileDto> GetProfileByIdAsync(string userId)
    {
        var user = await unitOfWork.UserRepository.GetByIdAsync(userId);
        var result = mapper.Map<UserProfileDto>(user);
        return result;
    }

    public async Task<bool> UpdateProfileAsync(string userId, UserProfileDto userProfileDto)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteProfileAsync(string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> AddProfileAsync(UserProfileDto userProfileDto)
    {
        var user = mapper.Map<User>(userProfileDto);
        await unitOfWork.UserRepository.AddAsync(user);
        await unitOfWork.CommitAsync();
        return true;
    }
}