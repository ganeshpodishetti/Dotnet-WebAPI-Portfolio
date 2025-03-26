using Application.DTOs.Experience;
using Domain.Entities;
using Profile = AutoMapper.Profile;

namespace Application.Mapping;

public class ExperienceProfile : Profile
{
    public ExperienceProfile()
    {
        CreateMap<Experience, ExperienceResponseDto>()
            .ForMember(dest => dest.UpdatedAtUtc, opt =>
                opt.MapFrom(src => src.UpdatedAt.ToString()));

        CreateMap<ExperienceRequestDto, Experience>();
    }
}