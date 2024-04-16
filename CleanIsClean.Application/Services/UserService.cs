using CleanIsClean.Domain.Interfaces;

namespace CleanIsClean.Application.Services;
public class UserService(IRepository<User> userRepository,
IRepository<UserRole> userRoleRepository,
IRepository<Role> roleRepository) : IUserService
{
    private readonly IRepository<User> _userRepository = userRepository;
    private readonly IRepository<UserRole> _userRoleRepository = userRoleRepository;
    private readonly IRepository<Role> _roleRepository = roleRepository;

    public async Task AddUserAsync(User user)
    {
        var role = await _roleRepository.Get(p => p.RoleName == "User");
        await _userRepository.AddAsync(user);
        await _userRoleRepository.AddAsync(new UserRole { UserId = user.Id, RoleId = role.First().RoleId });
    }
    public async Task<User?> GetUserByIdAsync(Guid? id)
    {
        IEnumerable<User?> users = await _userRepository.Get(u => u.Id == id);
        User? user = users.FirstOrDefault();
        return user;
    }
    public async Task<User?> GetUserByEmailAsync(string? email)
    {
        IEnumerable<User?> users = await _userRepository.Get(u => u.Email == email);
        User? user = users.FirstOrDefault();
        return user;
    }
    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _userRepository.Get();
    }
    public async Task UpdateUserAsync(User user)
    {
        await _userRepository.UpdateAsync(user);
    }
    public async Task DeleteUserByIdAsync(Guid? id)
    {
        await _userRepository.DeleteAsync(id);
    }

    public Task AddUserRole(User user, string roleName)
    {
        throw new NotImplementedException();
    }

    public async Task<string?> GetUserRoleNameByUserId(Guid? id)
    {
        IEnumerable<UserRole> userRoles = await _userRoleRepository.Get(p=> p.UserId == id, includeProperties: "Role");
        UserRole? userRole = userRoles.FirstOrDefault();
        return userRole?.Role?.RoleName;
    }
}
