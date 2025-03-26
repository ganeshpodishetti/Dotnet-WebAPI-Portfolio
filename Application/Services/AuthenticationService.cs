using System.Text.RegularExpressions;
using Application.DTOs.Authentication;
using Application.Interfaces;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Errors;
using Domain.Interfaces;
using Domain.Options;
using Microsoft.Extensions.Options;

namespace Application.Services;

public partial class AuthenticationService(
    IAuthenticationRepository authenticationRepository,
    IJwtTokenService jwtTokenService,
    IOptions<JwtTokenOptions> jwtOptions,
    IMapper mapper) : IAuthenticationService
{
    // register a new user
    public async Task<Result<RegisterResponseDto>> RegisterAsync(RegisterRequestDto request)
    {
        // Check for special characters and spaces
        if (request.UserName != SanitizeName(request.UserName!))
            return Result.Failure<RegisterResponseDto>(UserErrors.InvalidUserName(request.UserName));

        var existingUser = await authenticationRepository.FindByEmailAsync(request.Email);
        if (existingUser is not null)
            return Result.Failure<RegisterResponseDto>(UserErrors.UserAlreadyExists(existingUser.Email!));

        var user = mapper.Map<User>(request);

        var createdUser = await authenticationRepository.RegisterUserAsync(user, request.Password);

        var result = mapper.Map<RegisterResponseDto>(user);

        return createdUser.Match(
            _ => Result.Success(result),
            Result.Failure<RegisterResponseDto>
        );
    }

    // login a user
    public async Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto request)
    {
        var user = await authenticationRepository.FindByEmailAsync(request.Email);
        if (user is null)
            return Result.Failure<LoginResponseDto>(UserErrors.UserNotFound(request.Email));

        var validateUser = await authenticationRepository.ValidateCredentialsAsync(request.Email, request.Password);
        if (!validateUser)
            return Result.Failure<LoginResponseDto>(UserErrors.LoginFailed(request.Email));

        var token = await jwtTokenService.GenerateJwtToken(user!);
        var refreshToken = jwtTokenService.GenerateRefreshToken();

        var result = mapper.Map<LoginResponseDto>(user);
        result.AccessToken = token;
        result.RefreshToken = refreshToken;
        result.RefreshTokenExpirationAtUtc = DateTime.UtcNow.AddDays(jwtOptions.Value.RefreshTokenExpirationDays)
            .ToString("yyyy-MM-dd HH:mm:ss tt");
        result.AccessTokenExpirationAtUtc = DateTime.UtcNow.AddMinutes(jwtOptions.Value.AccessTokenExpirationMinutes)
            .ToString("yyyy-MM-dd HH:mm:ss tt");
        return result;
    }

    // change user password
    public async Task<Result<bool>> ChangePasswordAsync(ChangePasswordDto request, string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        var success = await authenticationRepository.ChangePasswordAsync(userId.ToString(), request.CurrentPassword,
            request.NewPassword);
        return success;
    }

    // delete a user
    public async Task<Result<bool>> DeleteUserAsync(string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        var success = await authenticationRepository.DeleteUserAsync(userId.ToString());
        return success;
    }

    // Find a user by username
    private static string SanitizeName(string name)
    {
        // Remove special characters and spaces
        return MyRegex().Replace(name, "");
    }

    // Regex for special characters and spaces
    [GeneratedRegex(@"[^a-zA-Z0-9.]")]
    private static partial Regex MyRegex();
}