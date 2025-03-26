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
        //CreateMap<Result<User>, Result<UserResponseDto>>();

        CreateMap<User, LoginResponseDto>();
        //CreateMap<Result<User>, Result<LoginResponseDto>>();

        CreateMap<User, RegisterResponseDto>()
            .ForMember(dest => dest.UserId, opt =>
                opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Email, opt =>
                opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.CreateAt, opt =>
                opt.MapFrom(src => src.CreatedAt.ToString("yyyy-M-d HH:mm:ss")));
        //CreateMap<Result<User>, Result<RegisterResponseDto>>();

        // Dtos to Domain
        CreateMap<LoginRequestDto, User>();

        CreateMap<RegisterRequestDto, User>()
            .ForMember(dest => dest.UserName, opt =>
                opt.MapFrom(src => src.UserName));

        CreateMap<UserRequestDto, AboutMe>()
            .ForMember(dest => dest.FirstName, opt =>
                opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt =>
                opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.ProfilePicture, opt =>
                opt.MapFrom(src => src.ProfilePicture))
            .ForMember(dest => dest.Bio, opt =>
                opt.MapFrom(src => src.Bio))
            .ForMember(dest => dest.Headline, opt =>
                opt.MapFrom(src => src.Headline))
            .ForMember(dest => dest.Country, opt =>
                opt.MapFrom(src => src.Country))
            .ForMember(dest => dest.City, opt =>
                opt.MapFrom(src => src.City));

        // Reverse mapping
        CreateMap<AboutMe, UserResponseDto>().ReverseMap();
        //CreateMap<Result<AboutMe>, Result<UserResponseDto>>().ReverseMap();
    }
}