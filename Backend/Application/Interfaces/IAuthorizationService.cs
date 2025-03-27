using Domain.Common;

namespace Application.Interfaces;

public interface IUserAuthorizationService
{
    // Task<Result<bool>> IsUserInRoleAsync(Guid userId, Guid roleId);
    Task<Result<IEnumerable<string>>> GetUserRolesAsync(string userId);
    Task<Result<bool>> AssignRoleAsync(string userId, string roleName);

    Task<Result<bool>> RemoveRoleAsync(string userId, string roleName);

    Task<Result<bool>> CreateRoleAsync(string roleName);
    // Task<Result<bool>> DeleteRoleAsync(Guid roleId);
    // Task<Result<bool>> IsRoleExistsAsync(string roleName);
    // Task<Result<bool>> IsRoleExistsAsync(Guid roleId);
    // Task<Result<bool>> IsUserExistsAsync(Guid userId);
}