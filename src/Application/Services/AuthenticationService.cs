using Application.DTOs.Authentication;
using Application.Interfaces;
using AutoMapper;
using Domain.Common;
using Domain.Common.ResultPattern;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.Interfaces;
using Domain.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.Services;

public class AuthenticationService(
    IAuthenticationRepository authenticationRepository,
    IJwtTokenService jwtTokenService,
    IOptions<JwtTokenOptions> jwtOptions,
    IMapper mapper,
    ILogger<AuthenticationService> logger,
    SanitizeName.ISanitizeName sanitizeName) : IAuthenticationService
{
    // register a new user
    public async Task<Result<RegisterResponseDto>> RegisterAsync(RegisterRequestDto request)
    {
        logger.LogInformation("Starting user registration process for email: {Email}", request.Email);

        var userName = sanitizeName.SanitizeNameAsync(request.UserName);
        if (userName)
        {
            logger.LogWarning("Registration failed: Invalid username format: {UserName}", request.UserName);
            return Result.Failure<RegisterResponseDto>(UserErrors.InvalidUserName(request.UserName));
        }

        // Check if an admin already exists
        var adminExists = await authenticationRepository.CheckAdminExistsAsync();
        if (adminExists.Value)
        {
            logger.LogWarning("Registration failed: Admin already exists. Attempted registration for email: {Email}",
                request.Email);
            return Result.Failure<RegisterResponseDto>(UserErrors.AdminAlreadyExists(request.Email!));
        }

        logger.LogDebug("Creating new user with email: {Email}", request.Email);
        var user = mapper.Map<User>(request);

        var createdUser = await authenticationRepository.RegisterUserAsync(user, request.Password);

        if (createdUser.IsSuccess)
        {
            logger.LogInformation("User {UserId} registered successfully with email: {Email}", user.Id, request.Email);
            logger.LogDebug("Assigning Admin role to first registered user: {UserId}", user.Id);
            await authenticationRepository.AssignRoleAsync(user, UserRole.Admin.ToString());
            logger.LogInformation("Admin role assigned successfully to user: {UserId}", user.Id);
        }
        else
        {
            logger.LogError("User registration failed for email: {Email}. Error: {Error}", request.Email,
                createdUser.Error);
        }

        var result = mapper.Map<RegisterResponseDto>(user);
        return Result<RegisterResponseDto>.Success(result);
    }

    // login a user
    public async Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto request)
    {
        logger.LogInformation("Processing login attempt for user: {Email}", request.Email);

        var user = await authenticationRepository.FindByEmailAsync(request.Email);
        if (user is null)
        {
            logger.LogWarning("Login failed: User not found for email: {Email}", request.Email);
            return Result.Failure<LoginResponseDto>(UserErrors.UserNotFound(request.Email));
        }

        logger.LogDebug("User found, attempting authentication for user ID: {UserId}", user.Id);
        var signInUser = await authenticationRepository.SignInUserAsync(user, request.Password);

        if (signInUser.IsFailure)
        {
            logger.LogWarning("Login failed: Invalid credentials for user: {Email}", request.Email);
            return Result.Failure<LoginResponseDto>(UserErrors.LoginFailed(request.Email));
        }

        logger.LogInformation("User {UserId} logged in successfully", user.Id);
        var authResponse = await GenerateAuthResponse(user);
        return Result<LoginResponseDto>.Success(authResponse);
    }

    // change user password
    public async Task<Result<bool>> ChangePasswordAsync(ChangePasswordDto request, string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        logger.LogInformation("Initiating password change for user ID: {UserId}", userId);

        var success = await authenticationRepository.ChangePasswordAsync(userId.ToString(), request.CurrentPassword,
            request.NewPassword);

        if (success.IsSuccess)
            logger.LogInformation("Password changed successfully for user ID: {UserId}", userId);
        else
            logger.LogWarning("Password change failed for user ID: {UserId}. Error: {Error}", userId, success.Error);

        return Result<bool>.Success(success.IsSuccess);
    }

    // delete a user
    public async Task<Result<bool>> DeleteUserAsync(string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        logger.LogWarning("Initiating user deletion for ID: {UserId}", userId);

        var success = await authenticationRepository.DeleteUserAsync(nameof(userId));

        if (success.IsSuccess)
            logger.LogInformation("User {UserId} deleted successfully", userId);
        else
            logger.LogError("Failed to delete user {UserId}. Error: {Error}", userId, success.Error);

        return Result<bool>.Success(success.IsSuccess);
    }

    // Token generation
    private async Task<LoginResponseDto> GenerateAuthResponse(User user)
    {
        logger.LogDebug("Generating authentication tokens for user ID: {UserId}", user.Id);
        var token = await jwtTokenService.GenerateJwtToken(user);
        var refreshToken = jwtTokenService.GenerateRefreshToken();

        var response = new LoginResponseDto(
            user.Id.ToString(),
            token,
            DateTime.UtcNow.AddMinutes(jwtOptions.Value.AccessTokenExpirationMinutes)
                .ToString("yyyy-MM-dd HH:mm:ss tt"),
            refreshToken,
            DateTime.UtcNow.AddDays(jwtOptions.Value.RefreshTokenExpirationDays).ToString("yyyy-MM-dd HH:mm:ss tt")
        );

        logger.LogDebug("Authentication tokens generated successfully for user ID: {UserId}", user.Id);
        return response;
    }
}