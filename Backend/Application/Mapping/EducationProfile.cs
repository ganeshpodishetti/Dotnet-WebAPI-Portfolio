using Application.DTOs.Education;
using Domain.Entities;
using Profile = AutoMapper.Profile;

namespace Application.Mapping;

public class EducationProfile : Profile
{
    public EducationProfile()
    {
        // Domain to Dtos
        CreateMap<Education, EducationResponseDto>()
            .ForMember(dest => dest.UpdatedAtUtc, opt =>
                opt.MapFrom(src => src.UpdatedAt.ToString()));

        // Dtos to Domain
        CreateMap<EducationRequestDto, Education>();
    }
}