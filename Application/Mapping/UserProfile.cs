using Application.DTOs;
using Domain.Entities;
using Profile = AutoMapper.Profile;

namespace Application.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserProfileDto>();
    }
}