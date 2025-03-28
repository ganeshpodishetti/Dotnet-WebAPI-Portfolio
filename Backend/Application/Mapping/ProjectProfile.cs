using Application.DTOs.Project;
using Domain.Entities;
using Profile = AutoMapper.Profile;

namespace Application.Mapping;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        // Dtos to Domain
        CreateMap<ProjectRequestDto, Project>();

        // Domain to Dtos
        CreateMap<Project, ProjectResponseDto>()
            .ConstructUsing(src => new ProjectResponseDto(
                src.Id.ToString(),
                src.Name,
                src.Description,
                src.Url,
                src.GithubUrl,
                src.UpdatedAt.ToString(),
                src.Skills
            ));
    }
}