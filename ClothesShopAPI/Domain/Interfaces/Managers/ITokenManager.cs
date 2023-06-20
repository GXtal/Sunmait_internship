using Domain.Entities;

namespace Domain.Interfaces.Managers;

public interface ITokenManager
{
    public string GenerateToken(User user);
}
