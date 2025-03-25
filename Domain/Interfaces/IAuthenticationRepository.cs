using Domain.Common;
using Domain.Entities;

namespace Domain.Interfaces;

public interface IAuthenticationRepository
{
    Task<Result<User>> RegisterUserAsync(User request, string password);
    Task<User> UpdateUserAsync(User user);
    Task<bool> ValidateCredentialsAsync(string email, string password);
    Task<User?> FindByEmailAsync(string email);
    Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
    Task<bool> DeleteUserAsync(string userId);
}