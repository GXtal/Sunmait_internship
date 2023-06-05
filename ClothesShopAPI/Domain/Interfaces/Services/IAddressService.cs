using Domain.Entities;

namespace Domain.Interfaces.Services;

public interface IAddressService
{
    public Task<IEnumerable<Address>> GetAddresses(int userId);

    public Task AddAddress(int userId, string fullAddress);

    public Task RemoveAddress(int id);
}
