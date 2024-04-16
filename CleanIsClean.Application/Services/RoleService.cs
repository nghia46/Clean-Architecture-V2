using CleanIsClean.Domain.Interfaces;
using CleanIsClean.Domain.ViewModels;

namespace CleanIsClean.Application.Services
{
    public class RoleService(IRepository<Role> roleRepository) : IRoleService
    {
        private readonly IRepository<Role> _roleRepository = roleRepository;

        public async Task<IEnumerable<Role>> GetAllRoles()
        {
            return await _roleRepository.Get();
        }
        public async Task CreateRole(RoleView role)
        {
            Role newRole= new()
            {
                RoleId = Guid.NewGuid(),
                RoleName = role.RoleName
            };
            await _roleRepository.AddAsync(newRole);
        }

        public async Task<Role?> GetRoleById(Guid id)
        {
            IEnumerable<Role> roles = await _roleRepository.Get(p=> p.RoleId == id);
            Role? role = roles.FirstOrDefault();
            return role;
        }
    }
}