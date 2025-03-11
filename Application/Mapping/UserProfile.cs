using Application.DTOs;
using Profile = AutoMapper.Profile;

namespace Application.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<Profile, UserProfileDto>();
    }
}