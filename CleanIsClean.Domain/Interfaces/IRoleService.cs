using CleanIsClean.Domain.ViewModels;

namespace CleanIsClean.Domain.Interfaces;
public interface IRoleService {
    Task<IEnumerable<Role>> GetAllRoles();
    Task<Role?> GetRoleById(Guid id);
    Task CreateRole(RoleView role);
}