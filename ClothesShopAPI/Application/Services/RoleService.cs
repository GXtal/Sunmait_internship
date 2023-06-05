using Application.Exceptions.Messages;
using Application.Exceptions;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;

namespace Application.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;
    public async Task<Role> GetRole(int id)
    {
        var role = await _roleRepository.GetRoleById(id);
        if (role == null)
        {
            throw new NotFoundException(String.Format(RoleExceptionsMessages.RoleNotFound, id));
        }
        return role;
    }

    public async Task<IEnumerable<Role>> GetRoles()
    {
        var roles = await _roleRepository.GetRoles();
        return roles;
    }
}
