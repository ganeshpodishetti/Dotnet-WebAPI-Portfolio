using Application.DTOs.Authentication;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Options;
using Microsoft.Extensions.Options;

namespace Application.Services;

public class AuthenticationService(
    IAuthenticationRepository authenticationRepository,
    IJwtTokenService jwtTokenService,
    IOptions<JwtTokenOptions> jwtOptions,
    IMapper mapper) : IAuthenticationService
{
    // register a new user
    public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        var existingUser = await authenticationRepository.FindByEmailAsync(request.Email);
        if (existingUser is not null)
            throw new UserAlreadyExistsException(existingUser.Email!);

        var user = mapper.Map<User>(request);

        var createdUser = await authenticationRepository.RegisterUserAsync(user, request.Password);

        var result = mapper.Map<RegisterResponseDto>(createdUser);
        return result;
    }

    // login a user
    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        var validateUser = await authenticationRepository.ValidateCredentialsAsync(request.Email, request.Password);
        if (!validateUser)
            throw new LoginFailedException(request.Email);

        var user = await authenticationRepository.FindByEmailAsync(request.Email);
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
        var user = await authenticationRepository.ChangePasswordAsync(userId.ToString(), request.CurrentPassword,
            request.NewPassword);
        return user;
    }

    // delete a user
    public async Task<bool> DeleteUserAsync(string accessToken)
    {
        var userId = jwtTokenService.GetUserIdFromToken(accessToken);
        var user = await authenticationRepository.DeleteUserAsync(userId.ToString());
        return user;
    }
}