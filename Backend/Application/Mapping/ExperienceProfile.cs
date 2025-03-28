using Application.DTOs.Experience;
using Domain.Entities;
using Profile = AutoMapper.Profile;

namespace Application.Mapping;

public class ExperienceProfile : Profile
{
    public ExperienceProfile()
    {
        // Domain to Dtos
        CreateMap<Experience, ExperienceResponseDto>()
            .ConstructUsing(src => new ExperienceResponseDto(
                src.Id.ToString(),
                src.Title,
                src.CompanyName,
                src.Location,
                src.StartDate,
                src.EndDate,
                src.Description,
                src.UpdatedAt.ToString()));


        // Dtos to Domain
        CreateMap<ExperienceRequestDto, Experience>();
    }
}