using Application.DTOs.Identity;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Options;
using Microsoft.Extensions.Options;

namespace Application.Services;

public class IdentityService(
    IIdentityRepository identityRepository,
    IJwtTokenService jwtTokenService,
    IOptions<JwtTokenOptions> jwtOptions,
    IMapper mapper) : IIdentityService
{
    // register a new user
    public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        var existingUser = await identityRepository.FindByEmailAsync(request.Email);
        if (existingUser != null) throw new UserAlreadyExistsException(request.Email, true);

        var user = mapper.Map<User>(request);

        var createdUser = await identityRepository.RegisterUserAsync(user, request.Password);

        var result = mapper.Map<RegisterResponseDto>(createdUser);
        return result;
    }

    // login a user
    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        var validateUser = await identityRepository.ValidateCredentialsAsync(request.Email, request.Password);
        if (!validateUser)
            throw new LoginFailedException(request.Email);

        var user = await identityRepository.FindByEmailAsync(request.Email);
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
    public async Task<bool> ChangePasswordAsync(ChangePasswordDto request, string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        var user = await identityRepository.ChangePasswordAsync(userId, request.CurrentPassword, request.NewPassword);
        return user;
    }
}