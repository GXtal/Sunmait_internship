using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ContactRepository : IContactRepository
{
    private readonly ShopDbContext _dbContext;

    public ContactRepository(ShopDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Contact> AddContact(Contact contact)
    {
        _dbContext.Add(contact);
        await Save();
        return contact;
    }

    public async Task<Contact> GetContactById(int id)
    {
        var contact = await _dbContext.Contacts.FirstOrDefaultAsync(c => c.Id == id);
        return contact;
    }

    public async Task<IEnumerable<Contact>> GetContactsByUser(User user)
    {
        var contacts = await _dbContext.Contacts.Where(c => c.UserId == user.Id).ToListAsync();
        return contacts;
    }

    public async Task RemoveContact(Contact contact)
    {
        _dbContext.Contacts.Remove(contact);
        await Save();
    }

    public async Task Save()
    {
        await _dbContext.SaveChangesAsync();
    }
}
