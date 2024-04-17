using System.Net;
using System.Text.Json;
using CleanIsClean.Domain.Interfaces;
using CleanIsClean.Domain.Models;
using CleanIsClean.Domain.ViewModels;
using Microsoft.AspNetCore.Http;

namespace CleanIsClean.Application.Services
{
    public class RoleService(IRepository<Role> roleRepository, IRepository<UserRole> userRoleRepository, IRepository<User> userRepository) : IRoleService
    {
        private readonly IRepository<User> _userRepository = userRepository;
        private readonly IRepository<UserRole> _userRoleRepository = userRoleRepository;
        private readonly IRepository<Role> _roleRepository = roleRepository;

        public async Task<IEnumerable<Role>> GetAllRoles()
        {
            return await _roleRepository.Get();
        }
        public async Task CreateRole(RoleView role)
        {
            Role newRole= new()
            {
                Id = Guid.NewGuid(),
                RoleName = role.RoleName
            };
            await _roleRepository.AddAsync(newRole);
        }

        public async Task<Role?> GetRoleById(Guid id)
        {
            IEnumerable<Role> roles = await _roleRepository.Get(p=> p.Id == id);
            Role? role = roles.FirstOrDefault();
            return role;
        }

        public async Task GrantPermissions(Guid userId, string roleName)
        {
            IEnumerable<User> users = await _userRepository.Get(p=> p.Id == userId);
            User? user = users.FirstOrDefault();

            IEnumerable<Role> roles = await _roleRepository.Get(p=> p.RoleName == roleName);
            Role? role = roles.FirstOrDefault();

            if(user == null || role == null) throw new Exception("User or Role not found");
            await _userRoleRepository.AddAsync(new UserRole { UserId = user.Id, RoleId = role.Id });
        }
    }
}