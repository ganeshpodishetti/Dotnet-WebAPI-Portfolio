using Application.DTOs;
using Application.DTOs.Identity;
using Domain.Entities;
using Profile = AutoMapper.Profile;

namespace Application.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserProfileDto>();
        CreateMap<User, LoginResponseDto>();
        CreateMap<User, RegisterResponseDto>()
            .ForMember(dest => dest.CreateAt, opt =>
                opt.MapFrom(src => src.CreatedAt.ToString("yyyy-M-d HH:mm:ss")));

        CreateMap<LoginRequestDto, User>();

        CreateMap<RegisterRequestDto, User>()
            .ForMember(dest => dest.UserName, opt =>
                opt.MapFrom(src => src.UserName));
        // .ForPath(dest => dest.Profile.FirstName, opt =>
        //     opt.MapFrom(src => src.FirstName))
        // .ForPath(dest => dest.Profile.LastName, opt =>
        //     opt.MapFrom(src => src.LastName));
    }
}