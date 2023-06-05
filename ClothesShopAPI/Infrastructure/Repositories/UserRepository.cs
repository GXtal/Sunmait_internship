using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ShopDbContext _dbContext;

    public UserRepository(ShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> AddUser(User user)
    {
        _dbContext.Add(user);
        await Save();
        return user;
    }

    public async Task<User> GetUserByEmail(string email)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        return user;
    }

    public async Task<User> GetUserById(int id)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        return user;
    }

    public async Task Save()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateUser(User user)
    {
        _dbContext.Entry(user).State = EntityState.Modified;
        await Save();
    }
}
