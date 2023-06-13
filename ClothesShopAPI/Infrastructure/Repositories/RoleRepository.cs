using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly ShopDbContext _dbContext;

    public RoleRepository(ShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Role> GetRoleById(int id)
    {
        var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Id == id);
        return role;
    }

    public async Task<IEnumerable<Role>> GetRoles()
    {
        var allRoles = await _dbContext.Roles.ToListAsync();
        return allRoles;
    }

    public async Task Save()
    {
        await _dbContext.SaveChangesAsync();
    }
}
