using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class AddressRepository : IAddressRepository
{
    private readonly ShopDbContext _dbContext;

    public AddressRepository(ShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Address> AddAddress(Address address)
    {
        _dbContext.Add(address);
        await Save();
        return address;
    }

    public async Task<Address> GetAddressById(int id)
    {
        var address = await _dbContext.Addresses.FirstOrDefaultAsync(a => a.Id == id);
        return address;
    }

    public async Task<IEnumerable<Address>> GetAddressesByUser(User user)
    {
        var addresses = await _dbContext.Addresses.Where(a => a.UserId == user.Id).ToListAsync();
        return addresses;
    }

    public async Task RemoveAddress(Address address)
    {
        _dbContext.Addresses.Remove(address);
        await Save();
    }

    public async Task Save()
    {
        await _dbContext.SaveChangesAsync();
    }
}
