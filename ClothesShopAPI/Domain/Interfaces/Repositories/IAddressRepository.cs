using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IAddressRepository
{
    public Task<Address> AddAddress(Address address);
    public Task<Address> GetAddressById(int id);
    public Task<IEnumerable<Address>> GetAddressesByUser(User user);
    public Task RemoveAddress(Address address);
}
