using Application.DTOs.Skill;
using Domain.Entities;
using Profile = AutoMapper.Profile;

namespace Application.Mapping;

public class SkillProfile : Profile
{
    public SkillProfile()
    {
        CreateMap<SkillRequestDto, Skill>();
        CreateMap<Skill, SkillResponseDto>()
            .ForMember(dest => dest.UpdatedAt, opt =>
                opt.MapFrom(src => src.UpdatedAt.ToString()));
    }
}