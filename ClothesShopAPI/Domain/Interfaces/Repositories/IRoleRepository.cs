using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IRoleRepository
{
    public Task<Role> GetRoleById(int id);

    public Task<IEnumerable<Role>> GetRoles();

    public Task Save();
}
