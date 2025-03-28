using Application.DTOs.SocialLink;
using Domain.Entities;
using Profile = AutoMapper.Profile;

namespace Application.Mapping;

public class SocialLinkProfile : Profile
{
    public SocialLinkProfile()
    {
        // Dtos to Domain
        CreateMap<SocialLinkRequestDto, SocialLink>();

        // Domain to Dtos
        CreateMap<SocialLink, SocialLinkResponseDto>()
            .ConstructUsing(src => new SocialLinkResponseDto(
                src.Id.ToString(),
                src.Platform,
                src.Url,
                src.Icon,
                src.UpdatedAt.ToString()
            ));
    }
}