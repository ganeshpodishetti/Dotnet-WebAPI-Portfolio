using Application.DTOs.Identity;
using Application.DTOs.User;
using Domain.Entities;
using Profile = AutoMapper.Profile;

namespace Application.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        // Domain to Dtos
        CreateMap<User, UserProfileDto>()
            .ForMember(dest => dest.FirstName, opt =>
                opt.MapFrom(src => src.Profile.FirstName))
            .ForMember(dest => dest.LastName, opt =>
                opt.MapFrom(src => src.Profile.LastName))
            .ForMember(dest => dest.ProfilePicture, opt =>
                opt.MapFrom(src => src.Profile.ProfilePicture))
            .ForMember(dest => dest.Bio, opt =>
                opt.MapFrom(src => src.Profile.Bio))
            .ForMember(dest => dest.Headline, opt =>
                opt.MapFrom(src => src.Profile.Headline))
            .ForMember(dest => dest.Country, opt =>
                opt.MapFrom(src => src.Profile.Country))
            .ForMember(dest => dest.City, opt =>
                opt.MapFrom(src => src.Profile.City));
        CreateMap<User, LoginResponseDto>();
        CreateMap<User, RegisterResponseDto>()
            .ForMember(dest => dest.CreateAt, opt =>
                opt.MapFrom(src => src.CreatedAt.ToString("yyyy-M-d HH:mm:ss")));

        // Dtos to Domain
        CreateMap<LoginRequestDto, User>();
        CreateMap<RegisterRequestDto, User>()
            .ForMember(dest => dest.UserName, opt =>
                opt.MapFrom(src => src.UserName));
        CreateMap<UserProfileDto, User>();

        // CreateMap<UserProfileDto, User>()
        //     .ForMember(d => d.Id, o => o.Ignore())
        //     .ForMember(d => d.CreatedAt, o => o.Ignore())
        //     .ForMember(d => d.UpdatedAt, o => o.Ignore());

        // Reverse mapping
        CreateMap<Domain.Entities.Profile, UserProfileDto>().ReverseMap();
    }
}