using Application.DTOs.Authentication;

namespace Application.Interfaces;

public interface IAuthenticationService
{
    Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request);
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    Task<bool> ChangePasswordAsync(ChangePasswordDto request, string accessToken);
    Task<bool> DeleteUserAsync(string accessToken);
}