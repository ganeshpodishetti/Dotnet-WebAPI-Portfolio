using Application.DTOs.SocialLink;
using Domain.Entities;
using Profile = AutoMapper.Profile;

namespace Application.Mapping;

public class SocialLinkProfile : Profile
{
    public SocialLinkProfile()
    {
        CreateMap<SocialLinkRequestDto, SocialLink>();
        CreateMap<SocialLink, SocialLinkResponseDto>()
            .ForMember(dest => dest.UpdateAt, opt =>
                opt.MapFrom(src => src.UpdatedAt.ToString()));
        ;
    }
}