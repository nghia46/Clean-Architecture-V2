namespace CleanIsClean.Domain.Interfaces;
public interface IUserService
{
    Task AddUserAsync(User user);
    Task<User?> GetUserByIdAsync(Guid? id);
    Task<User?> GetUserByEmailAsync(string? email);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task UpdateUserAsync(User user);
    Task DeleteUserByIdAsync(Guid? id);
    Task<string?> GetUserRoleNameByUserId(Guid? id);
    Task AddUserRole(User user, string roleName);
}