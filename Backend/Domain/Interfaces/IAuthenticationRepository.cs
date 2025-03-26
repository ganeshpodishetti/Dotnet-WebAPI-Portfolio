using Domain.Common;
using Domain.Entities;

namespace Domain.Interfaces;

public interface IAuthenticationRepository
{
    Task<Result<User>> RegisterUserAsync(User request, string password);
    Task<Result<bool>> UpdateUserAsync(User user);
    Task<bool> ValidateCredentialsAsync(string email, string password);
    Task<User?> FindByEmailAsync(string email);
    Task<Result<bool>> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
    Task<Result<bool>> DeleteUserAsync(string userId);
}