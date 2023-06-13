using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class StatusRepository : IStatusRepository
{
    private readonly ShopDbContext _dbContext;

    public StatusRepository(ShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Status> GetStatusById(int id)
    {
        var status = await _dbContext.Statuses.FirstOrDefaultAsync(s => s.Id == id);
        return status;
    }

    public async Task<IEnumerable<Status>> GetStatuses()
    {
        var allStatuses = await _dbContext.Statuses.ToListAsync();
        return allStatuses;
    }

    public async Task Save()
    {
        await _dbContext.SaveChangesAsync();
    }
}
