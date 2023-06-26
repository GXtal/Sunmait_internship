using Domain.Entities;

namespace Application.Interfaces;

public interface ITokenManager
{
    public string GenerateToken(User user);
}
