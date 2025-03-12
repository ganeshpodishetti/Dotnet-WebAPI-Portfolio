using Application.DTOs.Identity;

namespace Application.Interfaces;

public interface IIdentityService
{
    Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request);
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
}