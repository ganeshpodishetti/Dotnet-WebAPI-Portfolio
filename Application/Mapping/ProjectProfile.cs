using Application.DTOs.Project;
using Domain.Entities;
using Profile = AutoMapper.Profile;

namespace Application.Mapping;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<ProjectRequestDto, Project>();
        CreateMap<Project, ProjectResponseDto>()
            .ForMember(dest => dest.UpdatedAt, opt =>
                opt.MapFrom(src => src.UpdatedAt.ToString()));
    }
}