using Domain.Entities;

namespace Domain.Interfaces.Services;

public interface IUserService
{
    public Task<User> Register(string email, string passwordHash, string name, string surname);

    public Task<User> Login(string email, string passwordHash);

    public Task SetUserInfo(int id,string name, string surname);

    public Task<User> GetUserInfo(int id);
}
