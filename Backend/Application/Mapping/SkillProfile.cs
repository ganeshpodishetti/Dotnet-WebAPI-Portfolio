using Application.DTOs.Skill;
using Domain.Entities;
using Profile = AutoMapper.Profile;

namespace Application.Mapping;

public class SkillProfile : Profile
{
    public SkillProfile()
    {
        // Dtos to Domain
        CreateMap<SkillRequestDto, Skill>();

        // Domain to Dtos
        CreateMap<Skill, SkillResponseDto>()
            .ConstructUsing(src => new SkillResponseDto(
                src.Id.ToString(),
                src.SkillCategory,
                src.SkillsTypes,
                src.UpdatedAt.ToString()
            ));
    }
}