using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IContactRepository
{
    public Task<Contact> AddContact(Contact contact);
    public Task<Contact> GetContactById(int id);
    public Task<IEnumerable<Contact>> GetContactsByUser(User user);
    public Task RemoveContact(Contact contact);
}
