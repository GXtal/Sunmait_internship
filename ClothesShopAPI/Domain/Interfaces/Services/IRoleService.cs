using Domain.Entities;

namespace Domain.Interfaces.Services;
public interface IRoleService
{
    public Task<Role> GetRole(int id);

    public Task<IEnumerable<Role>> GetRoles();
}