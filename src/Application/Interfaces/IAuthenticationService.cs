using Application.DTOs.Authentication;
using Domain.Common.ResultPattern;

namespace Application.Interfaces;

public interface IAuthenticationService
{
    Task<Result<RegisterResponseDto>> RegisterAsync(RegisterRequestDto request);
    Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto request);
    Task<Result<bool>> ChangePasswordAsync(ChangePasswordDto request, string accessToken);
    Task<Result<bool>> DeleteUserAsync(string accessToken);
    Task<Result<bool>> LogoutAsync(string accessToken);
}