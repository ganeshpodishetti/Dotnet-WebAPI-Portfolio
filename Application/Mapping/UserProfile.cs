using Application.DTOs.Authentication;
using Application.DTOs.User;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        // Domain to Dtos
        CreateMap<User, UserResponseDto>()
            .ForMember(dest => dest.FirstName, opt =>
                opt.MapFrom(src => src.AboutMe.FirstName))
            .ForMember(dest => dest.LastName, opt =>
                opt.MapFrom(src => src.AboutMe.LastName))
            .ForMember(dest => dest.ProfilePicture, opt =>
                opt.MapFrom(src => src.AboutMe.ProfilePicture))
            .ForMember(dest => dest.Bio, opt =>
                opt.MapFrom(src => src.AboutMe.Bio))
            .ForMember(dest => dest.Headline, opt =>
                opt.MapFrom(src => src.AboutMe.Headline))
            .ForMember(dest => dest.Country, opt =>
                opt.MapFrom(src => src.AboutMe.Country))
            .ForMember(dest => dest.City, opt =>
                opt.MapFrom(src => src.AboutMe.City));
        CreateMap<User, LoginResponseDto>();
        CreateMap<User, RegisterResponseDto>()
            .ForMember(dest => dest.CreateAt, opt =>
                opt.MapFrom(src => src.CreatedAt.ToString("yyyy-M-d HH:mm:ss")));

        // Dtos to Domain
        CreateMap<LoginRequestDto, User>();
        CreateMap<RegisterRequestDto, User>()
            .ForMember(dest => dest.UserName, opt =>
                opt.MapFrom(src => src.UserName));
        CreateMap<UserRequestDto, User>();

        // Reverse mapping
        CreateMap<AboutMe, UserResponseDto>().ReverseMap();
    }
}