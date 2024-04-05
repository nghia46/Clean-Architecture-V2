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
    public async Task GetAllUsersAsync()
    {
        await _userRepository.GetAllAsync();
    }
}
