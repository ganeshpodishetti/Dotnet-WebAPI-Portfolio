using Application.DTOs.Education;
using Application.Interfaces;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Errors;
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
    public async Task<Result<bool>> AddEducationAsync(EducationRequestDto educationDto, string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);

        var existingUser = await unitOfWork.UserRepository.GetByIdAsync(userId);
        if (existingUser is null)
            return Result<bool>.Failure(EducationErrors.UserNotFoundToAddEducation(userId.ToString()));

        var education = mapper.Map<Education>(educationDto);
        education.UserId = existingUser.Id;

        var result = await unitOfWork.EducationRepository.AddAsync(education);
        await unitOfWork.CommitAsync();
        return Result<bool>.Success(result);
    }

    // update education
    public async Task<Result<bool>> UpdateEducationAsync(EducationRequestDto educationDto, Guid id, string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        var existingEducation = await unitOfWork.EducationRepository.GetByUserIdAsync(userId, id);
        if (existingEducation is null)
            return Result<bool>.Failure(EducationErrors.EducationNotBelongToUser(userId.ToString()));

        // Map DTO to existing entity to preserve id
        mapper.Map(educationDto, existingEducation);
        existingEducation.UpdatedAt = DateTime.UtcNow;

        var result = await unitOfWork.EducationRepository.UpdateAsync(existingEducation);
        await unitOfWork.CommitAsync();
        return Result<bool>.Success(result);
    }

    // delete education
    public async Task<Result<bool>> DeleteEducationAsync(Guid id, string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        var existingEducation = await unitOfWork.EducationRepository.GetByUserIdAsync(userId, id);
        if (existingEducation is null)
            return Result<bool>.Failure(EducationErrors.EducationNotBelongToUser(userId.ToString()));

        var result = await unitOfWork.EducationRepository.DeleteAsync(existingEducation);
        await unitOfWork.CommitAsync();
        return Result<bool>.Success(result);
    }

    // get education
    public async Task<Result<IEnumerable<EducationResponseDto>>> GetEducationsByIdAsync(string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);

        var educations = await unitOfWork.EducationRepository.GetAllByUserIdAsync(userId);
        if (educations is null)
            return Result<IEnumerable<EducationResponseDto>>.Failure(
                EducationErrors.EducationNotBelongToUser(userId.ToString()));

        var educationsList = mapper.Map<IEnumerable<EducationResponseDto>>(educations);
        return Result<IEnumerable<EducationResponseDto>>.Success(educationsList);
    }
}