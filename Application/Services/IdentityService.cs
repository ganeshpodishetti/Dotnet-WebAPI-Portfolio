using Application.DTOs.Identity;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class IdentityService(
    IIdentityRepository identityRepository,
    IJwtTokenService jwtTokenService,
    IMapper mapper) : IIdentityService
{
    public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        var email = request.Email ?? throw new Exception("Email is required");
        var existingUser = await identityRepository.FindByEmailAsync(email);
        if (existingUser != null) throw new Exception("User already exists");

        var user = mapper.Map<User>(request);

        var createdUser = await identityRepository.CreateUserAsync(user, request.Password);
        //var token = await jwtTokenService.GenerateJwtToken(createdUser);
        //var refreshToken = jwtTokenService.GenerateRefreshToken();

        var result = mapper.Map<RegisterResponseDto>(createdUser);
        return result;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        var validateUser = await identityRepository.ValidateCredentialsAsync(request.Email, request.Password);
        if (!validateUser)
            throw new Exception("Invalid credentials");

        var user = await identityRepository.FindByEmailAsync(request.Email);
        var token = await jwtTokenService.GenerateJwtToken(user!);
        var refreshToken = jwtTokenService.GenerateRefreshToken();

        var result = mapper.Map<LoginResponseDto>(user);
        result.AccessToken = token;
        result.RefreshToken = refreshToken;
        return result;
    }
}