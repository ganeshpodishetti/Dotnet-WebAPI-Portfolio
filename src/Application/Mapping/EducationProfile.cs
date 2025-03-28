using Application.DTOs.Education;
using Domain.Entities;
using Profile = AutoMapper.Profile;

namespace Application.Mapping;

public class EducationProfile : Profile
{
    public EducationProfile()
    {
        // Domain to Dtos
        // if you prefer keeping the primary constructor pattern, you can configure AutoMapper to use your constructor.
        CreateMap<Education, EducationResponseDto>()
            .ConstructUsing(src => new EducationResponseDto(
                src.Id.ToString(),
                src.School,
                src.Degree,
                src.FieldOfStudy,
                src.Location,
                src.StartDate,
                src.EndDate,
                src.Description,
                src.UpdatedAt.ToString()));

        // Dtos to Domain
        CreateMap<EducationRequestDto, Education>();
    }
}