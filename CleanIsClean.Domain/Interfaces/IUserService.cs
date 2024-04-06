using CleanIsClean.Domain.Models;
namespace CleanIsClean.Domain.Interfaces;
public interface IUserService
{
    Task AddUserAsync(User user);
    Task<User?> GetUserByIdAsync(int id);
    Task<User?> GetUserByEmailAsync(string email);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(int id);
}