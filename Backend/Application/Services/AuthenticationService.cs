using Application.DTOs.Authentication;
using Application.Interfaces;
using AutoMapper;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Domain.Errors;
using Domain.Interfaces;
using Domain.Options;
using Microsoft.Extensions.Options;

namespace Application.Services;

public class AuthenticationService(
    IAuthenticationRepository authenticationRepository,
    IJwtTokenService jwtTokenService,
    IOptions<JwtTokenOptions> jwtOptions,
    IMapper mapper,
    SanitizeName.ISanitizeName sanitizeName) : IAuthenticationService
{
    // register a new user
    public async Task<Result<RegisterResponseDto>> RegisterAsync(RegisterRequestDto request)
    {
        var userName = sanitizeName.SanitizeNameAsync(request.UserName);
        if (userName)
            return Result.Failure<RegisterResponseDto>(UserErrors.InvalidUserName(request.UserName));

        // Check if an admin already exists
        var adminExists = await authenticationRepository.CheckAdminExistsAsync();
        if (adminExists.Value)
            return Result.Failure<RegisterResponseDto>(UserErrors.AdminAlreadyExists(request.Email!));

        var user = mapper.Map<User>(request);

        var createdUser = await authenticationRepository.RegisterUserAsync(user, request.Password);

        if (createdUser.IsSuccess)
            // Assign Admin role to the first registered user
            await authenticationRepository.AssignRoleAsync(user, UserRole.Admin.ToString());

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

        var signInUser = await authenticationRepository.SignInUserAsync(user, request.Password);
        return signInUser.IsFailure
            ? Result.Failure<LoginResponseDto>(UserErrors.LoginFailed(request.Email))
            : Result.Success(await GenerateAuthResponse(user));
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

    // Token generation
    private async Task<LoginResponseDto> GenerateAuthResponse(User user)
    {
        var token = await jwtTokenService.GenerateJwtToken(user);
        var refreshToken = jwtTokenService.GenerateRefreshToken();

        return new LoginResponseDto(
            user.Id.ToString(), // userId
            token, // accessToken
            DateTime.UtcNow.AddMinutes(jwtOptions.Value.AccessTokenExpirationMinutes)
                .ToString("yyyy-MM-dd HH:mm:ss tt"), // accessTokenExpirationAtUtc
            refreshToken, // refreshToken
            DateTime.UtcNow.AddDays(jwtOptions.Value.RefreshTokenExpirationDays)
                .ToString("yyyy-MM-dd HH:mm:ss tt") // refreshTokenExpirationAtUtc
        );
    }
}