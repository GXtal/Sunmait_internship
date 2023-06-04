using Domain.Entities;

namespace Domain.Interfaces.Services;
public interface IUserService
{
    public Task<User> Register(string email, string passwordHash);

    public Task<User> Login(string email, string password);

    public Task SetUserInfo(int id,string name, string surname);

    public Task<User> GetUserInfo(int id);

    public Task<IEnumerable<Address>> GetAddresses(int id);

    public Task<IEnumerable<Contact>> GetContacts(int id);
}
