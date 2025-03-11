using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.UnitOfWork;

namespace Application.Services;

public class UserServices(IUnitOfWork unitOfWork, IMapper mapper)
    : IUserServices
{
    public async Task<UserProfileDto> GetProfileAsync()
    {
        var user = await unitOfWork.User.GetAllAsync();
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
        throw new NotImplementedException();
    }
}