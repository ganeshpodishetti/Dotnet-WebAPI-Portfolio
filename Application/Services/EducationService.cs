using Application.DTOs.Education;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.UnitOfWork;

namespace Application.Services;

public class EducationService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IJwtTokenService jwtTokenService)
    : IEducationService
{
    // add education
    public async Task<bool> AddEducationAsync(EducationRequestDto educationDto, string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);

        var existingUser = await unitOfWork.UserRepository.GetByIdAsync(userId);
        if (existingUser is null)
            throw new Exception("User does not exist to add education");

        var education = mapper.Map<Education>(educationDto);
        education.UserId = existingUser.Id;
        education.UpdatedAt = DateTime.UtcNow;

        var result = await unitOfWork.EducationRepository.AddAsync(education);
        await unitOfWork.CommitAsync();
        return result;
    }

    // update education
    public async Task<bool> UpdateEducationAsync(EducationRequestDto educationDto, string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        var existingEducation = await unitOfWork.EducationRepository.GetByUserIdAsync(userId);
        if (existingEducation is null)
            throw new Exception("User does not exist to update education.");

        // Map DTO to existing entity to preserve Id
        mapper.Map(educationDto, existingEducation);
        existingEducation.UpdatedAt = DateTime.UtcNow;

        var result = await unitOfWork.EducationRepository.UpdateAsync(existingEducation);
        await unitOfWork.CommitAsync();
        return result;
    }

    // delete education
    public async Task<bool> DeleteEducationAsync(string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        var existingEducation = await unitOfWork.EducationRepository.GetByUserIdAsync(userId);
        if (existingEducation is null)
            throw new Exception("User does not exist to delete education.");

        var result = await unitOfWork.EducationRepository.DeleteAsync(existingEducation);
        await unitOfWork.CommitAsync();
        return result;
    }

    // get education
    public async Task<List<EducationResponseDto>> GetEducationsByIdAsync(string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);

        var educations = await unitOfWork.EducationRepository.GetAllByUserIdAsync(userId);

        var result = mapper.Map<List<EducationResponseDto>>(educations);
        return result;
    }
}