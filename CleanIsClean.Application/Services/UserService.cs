using CleanIsClean.Domain.Interfaces;
using CleanIsClean.Infrastructure.Models;

namespace CleanIsClean.Application.Services;
public class UserService
{
    private readonly IRepository<User> _userRepository;

    public UserService(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task AddUserAsync(User user)
    {
        await _userRepository.AddAsync(user);
    }
    public async Task<User?> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user;
    }
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        IEnumerable<User?> users = await _userRepository.Get(u => u.Email == email);
        User? user = users.FirstOrDefault();
        return user;
    }
    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
       return await _userRepository.Get();
    }
}
