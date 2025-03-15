using Application.DTOs.Education;
using Domain.Entities;
using Profile = AutoMapper.Profile;

namespace Application.Mapping;

public class EducationProfile : Profile
{
    public EducationProfile()
    {
        // Domain to Dtos
        CreateMap<Education, EducationResponseDto>();

        // Dtos to Domain
        CreateMap<EducationRequestDto, Education>()
            .ForMember(dest => dest.Id, opt => opt
                .Ignore())
            .ForMember(dest => dest.UserId, opt => opt
                .Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt
                .Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt
                .Ignore())
            .ForMember(dest => dest.StartDate, opt => opt
                .MapFrom(src => DateOnly.ParseExact(src.StartDate, "yyyy-MM-dd")))
            .ForMember(dest => dest.EndDate, opt => opt
                .MapFrom(src => DateOnly.ParseExact(src.EndDate ?? string.Empty, "yyyy-MM-dd")));
    }
}