using Domain.Entities;

namespace Domain.Interfaces.Services;

public interface IContactService
{
    public Task<IEnumerable<Contact>> GetContacts(int userId);

    public Task AddContact(int userId, string phoneNumber);

    public Task RemoveContact(int id);
}
